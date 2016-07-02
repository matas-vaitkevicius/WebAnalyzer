/****** Script for SelectTopNRows command from SSMS  ******/
select * from (SELECT (s.sq/r.r_sq)/12 years, * FROM (select  SUBSTRING(Title,1,4) miestas, 
SUBSTRING(Title, CHARINDEX(',', Title, 0)+2, CASE WHEN CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 < 0 then 0 else CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 end) rajonas,
 sum(price)/sum(LivingArea) sq , coalesce(sl.RoomCount,-1) roomC, count(1) cnt , sum(case when sl.DateRemoved is not null then 1 else 0 end) taken, cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  FROM [WebAnalyzer].[dbo].sale sl
  where url like '%aruodas%'
  and HeatingType not like '%Kietu kuru%'
group by SUBSTRING(Title,1,4) , 
SUBSTRING(Title, CHARINDEX(',', Title, 0)+2, CASE WHEN CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 < 0 then 0 else CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 end) 
 , RoomCount) s
 join

(select  SUBSTRING(Title,1,4) r_miestas, 
SUBSTRING(Title, CHARINDEX(',', Title, 0)+2, CASE WHEN CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 < 0 then 0 else CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 end)  r_rajonas, 
sum(price)/sum(LivingArea) r_sq , coalesce(RoomCount,-1) r_roomC, count(1) r_cnt , sum(case when DateRemoved is not null then 1 else 0 end) r_taken, cast(sum(case when DateRemoved is not null then 1 else 0 end) as decimal)/count(1) r_taken_ratio
  FROM [WebAnalyzer].[dbo].Rent
  where url like '%aruodas%'
  and HeatingType not like '%Kietu kuru%'
group by SUBSTRING(Title,1,4) ,
SUBSTRING(Title, CHARINDEX(',', Title, 0)+2, CASE WHEN CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 < 0 then 0 else CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 end) 
, RoomCount) r on r.r_roomC = s.roomC and r.r_miestas = s.miestas and r.r_rajonas = s.rajonas) by_dist
order by years 