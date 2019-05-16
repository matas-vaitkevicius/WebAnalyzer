/****** Script for SelectTopNRows command from SSMS  ******/
--square meter to rental square meter price ratio

select *, (s.sq/r.sq)/12 years from (SELECT  
title , sum(price)/sum(LivingArea) sq , sl.RoomCount, count(1) cnt , sum(case when sl.DateRemoved is not null then 1 else 0 end) taken, cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  FROM [WebAnalyzer].[dbo].[Sale] sl
  group by title, RoomCount ) s
  join (SELECT  
title, sum(price)/sum(LivingArea) sq ,RoomCount, count(1) cnt, sum(case when DateRemoved is not null then 1 else 0 end) taken, cast(sum(case when DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  FROM [WebAnalyzer].[dbo].Rent 
  group by title , RoomCount ) r on r.title = s.title and r.RoomCount = s.RoomCount
  where r.cnt > 3 and s.cnt > 3
  --Subtitle like '%Rott%'
  --and
 --  DateRemoved is null
  --order by price  
  order by years desc