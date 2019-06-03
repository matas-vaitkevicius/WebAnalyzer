/****** Script for SelectTopNRows command from SSMS  ******/
--square meter to rental square meter price ratio
select analysis.cnt,
--sa.Salesin1kradiusCount,
case when sa.Salesin100radiusCount>=4 then (ads.price/ads.LivingArea)/sa.Salesin100radiusAvgSqM else (ads.price/ads.LivingArea)/sa.Salesin200radiusAvgSqM  end local_ratio,
--(ads.price/ads.LivingArea)/(analysis.sq) avg_town_ratio, 
(ads.price/ads.LivingArea) per_sq_m, 
--sa.Salesin500radiusCount,
sa.Salesin200radiusCount,sa.Salesin100radiusCount, 
--analysis.sq, 
--sa.Salesin1kradiusAvgSqM,
--sa.Salesin500radiusAvgSqM,
sa.Salesin200radiusAvgSqM,sa.Salesin100radiusAvgSqM, analysis.RoomCount, --analysis.r_sq,

 --(ads.LivingArea*analysis.r_sq) rent_per_month, (ads.LivingArea*analysis.sq) worth, 
 --case when sa.REntsin100radiusCount>=3 then ((ads.Price/(ads.LivingArea*sa.Rentsin100radiusAvgSqM))/12) else 
 --((ads.Price/(ads.LivingArea*sa.Rentsin200radiusAvgSqM))/12) 
 -- end local_years,
  ((ads.Price/(ads.LivingArea*sa.Rentsin500radiusAvgSqM))/12) l500years,
  Rentsin500radiusCount ,
 --  case when sa.REntsin100radiusCount>=3 then sa.REntsin100radiusCount else sa.REntsin200radiusCount  end RentCnt,
 -- case when sa.REntsin100radiusCount>=3 then ((ads.Price/(ads.LivingArea*sa.Rentsin100radiusAvgSqM))/12)-((ads.Price/(ads.LivingArea*sa.Rentsin500radiusAvgSqM))/12) else 
 --((ads.Price/(ads.LivingArea*sa.Rentsin200radiusAvgSqM))/12) -((ads.Price/(ads.LivingArea*sa.Rentsin500radiusAvgSqM))/12)
 -- end err,
 --(ads.Price/(ads.LivingArea*analysis.r_sq))/12 years_to_pay_for_itself,
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
  inner join [WebAnalyzer].[dbo].SpatialAnalysis sa on ads.id = sa.saleid
 where ads.LivingArea is not null
 
 --and ads.title in ( 'gandia','daimus')
 --and local_years is not null
 and ads.Price < 56000
 and case when sa.Salesin100radiusCount>=4 then (ads.price/ads.LivingArea)/sa.Salesin100radiusAvgSqM else (ads.price/ads.LivingArea)/sa.Salesin200radiusAvgSqM  end <=0.7
 and ads.DateLastProcessed > '2019-05-25'
 --and ads.DateAdded < '2019-05-10'
  --Subtitle like '%Rott%'
  --and
 --  DateRemoved is null
  --order by price  
  order by l500years --per_sq_m
