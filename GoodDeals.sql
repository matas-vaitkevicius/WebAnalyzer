/****** Script for SelectTopNRows command from SSMS  ******/
--square meter to rental square meter price ratio

select (ads.price/ads.LivingArea) per_sq_m, (ads.price/ads.LivingArea)/(an.sq) avg_ratio, *, PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) town from Sale ads
left outer join (

select *, (s.sq/r.r_sq)/12 years from (SELECT  
PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) name , sum(price)/sum(LivingArea) sq , coalesce(sl.RoomCount,-1) Roomcount, count(1) cnt , sum(case when sl.DateRemoved is not null then 1 else 0 end) taken, cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  FROM [WebAnalyzer].[dbo].[Sale] sl
  group by PARSENAME(REPLACE(Subtitle, ' ', '.'), 1), coalesce(sl.RoomCount,-1) ) s
  join (SELECT  
PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) r_name, sum(price)/sum(LivingArea) r_sq ,coalesce(RoomCount,-1) r_roomcount, count(1) r_cnt, sum(case when DateRemoved is not null then 1 else 0 end) r_taken, cast(sum(case when DateRemoved is not null then 1 else 0 end) as decimal)/count(1) r_taken_ratio
  FROM [WebAnalyzer].[dbo].Rent 
  group by PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) , coalesce(RoomCount,-1) ) r on r.r_name = s.name and coalesce(r.r_roomcount,-1) = coalesce(s.RoomCount,-1)) an on an.name = PARSENAME(REPLACE(ads.Subtitle, ' ', '.'), 1) and coalesce(an.RoomCount,-1)  = coalesce(ads.RoomCount,-1)
  where 
  (ads.price/ads.LivingArea) < (an.sq)*0.7 
  and ads.DateRemoved is  null
  and price < 100000
  and ads.ServiceCosts < 200
  --Subtitle like '%Rott%'
  --and
 --  DateRemoved is null
  --order by price  
  order by (ads.price/ads.LivingArea)/(an.sq) desc