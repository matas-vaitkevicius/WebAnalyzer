select 
* from (
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