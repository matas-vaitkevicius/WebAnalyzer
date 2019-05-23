/****** Script for SelectTopNRows command from SSMS  ******/
--square meter to rental square meter price ratio
select analysis.cnt, analysis.RoomCount, analysis.r_sq,
(ads.price/ads.LivingArea) per_sq_m, (ads.price/ads.LivingArea)/(analysis.sq) avg_town_ratio, 
 (ads.LivingArea*analysis.r_sq) rent_per_month, (ads.LivingArea*analysis.sq) worth, (ads.Price/(ads.LivingArea*analysis.r_sq))/12 years_to_pay_for_itself,
 url, price,* from (
select s.title, s.roomcount, s.cnt, (s.sq/r.r_sq)/12 years, s.sq, r.r_sq from (SELECT  
title , sum(price)/sum(LivingArea) sq ,   sl.RoomCount, count(1) cnt , sum(case when sl.DateRemoved is not null then 1 else 0 end) taken, cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  FROM [WebAnalyzer].[dbo].[Sale] sl
  group by title, RoomCount ) s
  join (SELECT  
title, sum(price)/sum(LivingArea) r_sq ,RoomCount, count(1) cnt, sum(case when DateRemoved is not null then 1 else 0 end) taken, cast(sum(case when DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  FROM [WebAnalyzer].[dbo].Rent 
  group by title , RoomCount ) r on r.title = s.title and r.RoomCount = s.RoomCount
  where r.cnt > 3 and s.cnt > 3) analysis
  inner join [WebAnalyzer].[dbo].Sale ads on analysis.Title =ads.Title  and analysis.RoomCount  = coalesce(ads.RoomCount,-1) 
 where ads.LivingArea is not null
 
 and ads.title = 'gandia'
 and ads.Price < 56000
 and ads.DateAdded > '2019-05-18'
  --Subtitle like '%Rott%'
  --and
 --  DateRemoved is null
  --order by price  
  order by per_sq_m
