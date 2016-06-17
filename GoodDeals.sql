
select analysis.cnt, analysis.l_cnt, (ads.price/ads.LivingArea) per_sq_m, (ads.price/ads.LivingArea)/(analysis.sq) avg_town_ratio, (ads.price/ads.LivingArea)/(analysis.l_sq) avg_local_ratio, ads.Price/(((ads.LivingArea*analysis.l_r_sq)-ads.ServiceCosts)*12) years_to_pay_for_itself, *, PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) town from Sale ads
left outer join (
select * from (select *, (s.sq/r.r_sq)/12 years from (SELECT  
PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) name , sum(price)/sum(LivingArea) sq , coalesce(sl.RoomCount,-1) roomC, count(1) cnt , sum(case when sl.DateRemoved is not null then 1 else 0 end) taken, cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  FROM [WebAnalyzer].[dbo].[Sale] sl
  group by PARSENAME(REPLACE(Subtitle, ' ', '.'), 1), coalesce(RoomCount,-1) ) s
  join (SELECT  
PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) r_name, sum(price)/sum(LivingArea) r_sq ,coalesce(RoomCount,-1) r_roomcount, count(1) r_cnt, sum(case when DateRemoved is not null then 1 else 0 end) r_taken, cast(sum(case when DateRemoved is not null then 1 else 0 end) as decimal)/count(1) r_taken_ratio
  FROM [WebAnalyzer].[dbo].Rent 
  group by PARSENAME(REPLACE(Subtitle, ' ', '.'), 1) , coalesce(RoomCount,-1) ) r on r.r_name = s.name and r.r_roomcount = s.roomC) by_town
  left outer join
 (select *, (s.l_sq/r.l_r_sq)/12 l_years from (SELECT  
PARSENAME(REPLACE(sl.Subtitle, ' ', '.'), 1) l_name,
dbo.PullNumbers(Subtitle) postarea, 
sum(price)/sum(LivingArea) l_sq , 
coalesce(sl.RoomCount,-1) l_roomC, 
count(1) l_cnt , 
sum(case when sl.DateRemoved is not null then 1 else 0 end) l_taken, 
cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) l_taken_ratio
  FROM [WebAnalyzer].[dbo].[Sale] sl
  --where PostCode LIKE '%9736%' or PostCode LIKE '%9737%'
  group by PARSENAME(REPLACE(sl.Subtitle, ' ', '.'), 1), dbo.PullNumbers(Subtitle) , coalesce(sl.RoomCount,-1) ) s
  full join (SELECT  
  PARSENAME(REPLACE(sl.Subtitle, ' ', '.'), 1) l_r_name,
dbo.PullNumbers(Subtitle) l_r_postarea, 
sum(price)/sum(LivingArea) l_r_sq , 
coalesce(sl.RoomCount,-1) l_r_roomcount, 
count(1) l_r_cnt , 
sum(case when sl.DateRemoved is not null then 1 else 0 end) l_r_taken, 
cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) l_r_taken_ratio
  FROM [WebAnalyzer].[dbo].[Rent] sl
  --where PostCode LIKE '%9736%' or PostCode LIKE '%9737%'
  group by PARSENAME(REPLACE(sl.Subtitle, ' ', '.'), 1), dbo.PullNumbers(Subtitle) , coalesce(sl.RoomCount,-1)) r 
  on r.l_r_name = s.l_name and ((r.l_r_postarea = s.postarea and r.l_r_roomcount = s.l_roomC ))) by_area 


 on (by_town.name = by_area.l_name and by_town.roomC = by_area.l_roomC) or (by_town.name = by_area.l_r_name and by_town.roomC = by_area.l_r_roomcount) ) analysis
   on analysis.postarea = dbo.PullNumbers(Subtitle) and analysis.roomC  = coalesce(ads.RoomCount,-1)
  where 
 --((ads.price/ads.LivingArea) < (analysis.l_sq)*0.8 or (ads.price/ads.LivingArea) < (analysis.sq)*0.7) and
   ads.DateRemoved is  null and
   price < 100000 and
   ads.ServiceCosts < 150
   and analysis.l_cnt >3
 -- --Subtitle like '%Rott%'
 -- --and
 ----  DateRemoved is null
 --order by price  
 -- order by (ads.price/ads.LivingArea)/(an.sq) desc
 order by years_to_pay_for_itself