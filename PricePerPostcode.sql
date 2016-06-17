
--select (ads.price/ads.LivingArea) per_sq_m, (ads.price/ads.LivingArea)/(an.sq) avg_ratio, *, PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) town from Sale ads
--left outer join (

CREATE FUNCTION dbo.PullNumbers
(@strAlphaNumeric VARCHAR(256))
RETURNS VARCHAR(256)
AS
BEGIN
DECLARE @intAlpha INT
SET @intAlpha = PATINDEX('%[^0-9]%', @strAlphaNumeric)
BEGIN
WHILE @intAlpha > 0
BEGIN
SET @strAlphaNumeric = STUFF(@strAlphaNumeric, @intAlpha, 1, '' )
SET @intAlpha = PATINDEX('%[^0-9]%', @strAlphaNumeric )
END
END
RETURN ISNULL(@strAlphaNumeric,0)
END
GO

--select (ads.price/ads.LivingArea) per_sq_m, (ads.price/ads.LivingArea)/(an.sq) avg_ratio, *, PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) town from Sale ads
--left outer join (
select * from (select *, (s.sq/r.r_sq)/12 years from (SELECT  
PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) name , sum(price)/sum(LivingArea) sq , coalesce(sl.RoomCount,-1) roomC, count(1) cnt , sum(case when sl.DateRemoved is not null then 1 else 0 end) taken, cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  FROM [WebAnalyzer].[dbo].[Sale] sl
  group by PARSENAME(REPLACE(Subtitle, ' ', '.'), 1), coalesce(RoomCount,-1) ) s
  join (SELECT  
PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) r_name, sum(price)/sum(LivingArea) r_sq ,coalesce(RoomCount,-1) r_roomcount, count(1) r_cnt, sum(case when DateRemoved is not null then 1 else 0 end) r_taken, cast(sum(case when DateRemoved is not null then 1 else 0 end) as decimal)/count(1) r_taken_ratio
  FROM [WebAnalyzer].[dbo].Rent 
  group by PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) , coalesce(RoomCount,-1) ) r on r.r_name = s.name and r.r_roomcount = s.roomC) by_town
  left outer join
 (select *, (s.sq/r.r_sq)/12 years from (SELECT  
PARSENAME(REPLACE(sl.Subtitle, ' ', '.'), 1) name,
dbo.PullNumbers(Subtitle) postarea, 
sum(price)/sum(LivingArea) sq , 
coalesce(sl.RoomCount,-1) roomC, 
count(1) cnt , 
sum(case when sl.DateRemoved is not null then 1 else 0 end) taken, 
cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  FROM [WebAnalyzer].[dbo].[Sale] sl
  --where PostCode LIKE '%9736%' or PostCode LIKE '%9737%'
  group by PARSENAME(REPLACE(sl.Subtitle, ' ', '.'), 1), dbo.PullNumbers(Subtitle) , coalesce(sl.RoomCount,-1) ) s
  full join (SELECT  
  PARSENAME(REPLACE(sl.Subtitle, ' ', '.'), 1) r_name,
dbo.PullNumbers(Subtitle) r_postarea, 
sum(price)/sum(LivingArea) r_sq , 
coalesce(sl.RoomCount,-1) r_roomcount, 
count(1) r_cnt , 
sum(case when sl.DateRemoved is not null then 1 else 0 end) r_taken, 
cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) r_taken_ratio
  FROM [WebAnalyzer].[dbo].[Rent] sl
  --where PostCode LIKE '%9736%' or PostCode LIKE '%9737%'
  group by PARSENAME(REPLACE(sl.Subtitle, ' ', '.'), 1), dbo.PullNumbers(Subtitle) , coalesce(sl.RoomCount,-1)) r 
  on r.r_name = s.name and ((r.r_postarea = s.postarea and r.r_roomcount = s.roomC ))) by_area


 on (by_town.name = by_area.name and by_town.roomC = by_area.roomC) or (by_town.name = by_area.r_name and by_town.roomC = by_area.r_roomcount)
 --  on an.name = PARSENAME(REPLACE(ads.Subtitle, ' ', '.'), 1) and an.RoomCount  = ads.RoomCount
--  where 
 --(by_town.sq) < (an.sq)*0.7 and
 --  ads.DateRemoved is  null and
 --  price < 100000 and
 --  ads.ServiceCosts < 200
 -- --Subtitle like '%Rott%'
 -- --and
 ----  DateRemoved is null
 --order by price  
 -- order by (ads.price/ads.LivingArea)/(an.sq) desc
 order by by_area.years