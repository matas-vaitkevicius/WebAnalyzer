/****** Script for SelectTopNRows command from SSMS  ******/
SET ARITHABORT OFF 
SET ANSI_WARNINGS OFF

select analysis.cnt, analysis.l_cnt, 
(ads.price/ads.LivingArea) per_sq_m, (ads.price/ads.LivingArea)/(analysis.sq) avg_town_ratio, (ads.price/ads.LivingArea)/(analysis.l_sq) avg_local_ratio, (ads.LivingArea*analysis.l_r_sq) rent_per_month, (ads.LivingArea*analysis.l_sq) worth, (ads.Price/(ads.LivingArea*analysis.l_r_sq))/12 years_to_pay_for_itself,
 url,* from (
select * from
  (SELECT (s.l_sq/r.l_r_sq)/12 l_years, * FROM (select  SUBSTRING(Title,1,4) miestas, 
SUBSTRING(Title, CHARINDEX(',', Title, 0)+2, CASE WHEN CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 < 0 then 0 else CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 end) rajonas,
 sum(price)/sum(LivingArea) l_sq , coalesce(sl.RoomCount,-1) l_roomC, count(1) l_cnt , sum(case when sl.DateRemoved is not null then 1 else 0 end) l_taken, cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) l_taken_ratio
 ,IsBendrabutis l_isb
  FROM [WebAnalyzer].[dbo].sale sl
  where url like '%aruodas%'
  and HeatingType not like '%Kietu kuru%'
group by SUBSTRING(Title,1,4) , 
SUBSTRING(Title, CHARINDEX(',', Title, 0)+2, CASE WHEN CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 < 0 then 0 else CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 end) 
 , RoomCount, IsBendrabutis) s
 join

(select  SUBSTRING(Title,1,4) l_r_miestas, 
SUBSTRING(Title, CHARINDEX(',', Title, 0)+2, CASE WHEN CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 < 0 then 0 else CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 end)  l_r_rajonas, 
sum(price)/sum(LivingArea) l_r_sq , coalesce(RoomCount,-1) l_r_roomC, count(1) l_r_cnt , sum(case when DateRemoved is not null then 1 else 0 end) l_r_taken, cast(sum(case when DateRemoved is not null then 1 else 0 end) as decimal)/count(1) l_r_taken_ratio
 , IsBendrabutis l_r_isb
  FROM [WebAnalyzer].[dbo].Rent
  where url like '%aruodas%'
  and HeatingType not like '%Kietu kuru%'
group by SUBSTRING(Title,1,4) ,
SUBSTRING(Title, CHARINDEX(',', Title, 0)+2, CASE WHEN CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 < 0 then 0 else CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 end) 
, RoomCount, IsBendrabutis) r on r.l_r_roomC = s.l_roomC and r.l_r_miestas = s.miestas and r.l_r_rajonas = s.rajonas and r.l_r_isb = s.l_isb) by_dist
join (select  * from 
  (SELECT (s.sq/r.r_sq)/12 years, * FROM (
  SELECT  SUBSTRING(Title,1,4) name, sum(price)/sum(LivingArea) sq , coalesce(sl.RoomCount,-1) roomC, count(1) cnt , sum(case when sl.DateRemoved is not null then 1 else 0 end) taken, cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  ,IsBendrabutis isb
  FROM [WebAnalyzer].[dbo].[Sale] sl
  where url like '%aruodas%'   and HeatingType not like '%Kietu kuru%'
  group by  SUBSTRING(Title,1,4), coalesce(RoomCount,-1) ,IsBendrabutis) s
  join (SELECT   SUBSTRING(Title,1,4) r_name, sum(price)/sum(LivingArea) r_sq ,coalesce(RoomCount,-1) r_roomcount, count(1) r_cnt, sum(case when DateRemoved is not null then 1 else 0 end) r_taken, cast(sum(case when DateRemoved is not null then 1 else 0 end) as decimal)/count(1) r_taken_ratio
  , IsBendrabutis r_isb
  FROM [WebAnalyzer].[dbo].Rent 
  where url like '%aruodas%'   and HeatingType not like '%Kietu kuru%'
  group by  SUBSTRING(Title,1,4), coalesce(RoomCount,-1) , IsBendrabutis) r on r.r_name = s.name and r.r_roomcount = s.roomC and r.r_isb = s.isb) by_town) bt on bt.name = by_dist.miestas and bt.roomC = by_dist.l_roomC and bt.isb = by_dist.l_isb ) analysis
  inner join Sale ads on analysis.miestas = SUBSTRING(Title,1,4) and analysis.rajonas = SUBSTRING(Title, CHARINDEX(',', Title, 0)+2, CASE WHEN CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 < 0 then 0 else CHARINDEX(',', Title, CHARINDEX(',', Title, 0)+1 ) - CHARINDEX(',', Title, 0) - 2 end)  and analysis.roomC  = coalesce(ads.RoomCount,-1) 
  and ads.IsBendrabutis = analysis.isb
  where 

 ((
-- (ads.price/ads.LivingArea) > (analysis.l_sq)*0.6 and 
 (ads.price/ads.LivingArea) < (analysis.l_sq)*1) --or (ads.price/ads.LivingArea) < (analysis.sq)*0.5
 ) 
 and ads.DateRemoved is  null  
 and Price < 20000
 and miestas in ('Viln','Šiau')


and analysis.l_cnt > 3 and analysis.l_r_cnt > 3 and analysis.l_roomC > 0 
   and HeatingType not like '%Kietu kuru%'

  -- and Id not in (10251,12510,9031,12996,14838,8668,13440,13947,12998,15217,13079,8627,13827,13013,13680,13146,14946,12342,14472,9022,13055,12517,13154,15206,13587,14783,13806,14431,13804,14416,12499,12527,6312,6308,7156,13177,15128,14399,13096,11519)

order by avg_local_ratio desc 

----http://www.aruodas.lt/butai-vilniuje-naujojoje-vilnioje-naujoji-g-parduodamas-kambarys-bendrabutyje-naujojoje-g-1-2075029/
----http://www.aruodas.lt/butai-siauliuose-centre-vytauto-g-dvieju-kambariu-butas-vytauto-gatveje-1-2077963/

----lievi
----http://www.aruodas.lt/butai-siauliuose-centre-medelyno-g-parduodamas-4452kvm-butas-su-atskiru-iejimu-1-2011825/