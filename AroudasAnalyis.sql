select  * from 
  (SELECT (s.sq/r.r_sq)/12 years, * FROM (
  SELECT  SUBSTRING(Title,1,4) name, sum(price)/sum(LivingArea) sq , coalesce(sl.RoomCount,-1) roomC, count(1) cnt , sum(case when sl.DateRemoved is not null then 1 else 0 end) taken, cast(sum(case when sl.DateRemoved is not null then 1 else 0 end) as decimal)/count(1) taken_ratio
  FROM [WebAnalyzer].[dbo].[Sale] sl
  where url like '%aruodas%'
  group by  SUBSTRING(Title,1,4), coalesce(RoomCount,-1) ) s
  join (SELECT   SUBSTRING(Title,1,4) r_name, sum(price)/sum(LivingArea) r_sq ,coalesce(RoomCount,-1) r_roomcount, count(1) r_cnt, sum(case when DateRemoved is not null then 1 else 0 end) r_taken, cast(sum(case when DateRemoved is not null then 1 else 0 end) as decimal)/count(1) r_taken_ratio
  FROM [WebAnalyzer].[dbo].Rent 
  where url like '%aruodas%'
  group by  SUBSTRING(Title,1,4), coalesce(RoomCount,-1) ) r on r.r_name = s.name and r.r_roomcount = s.roomC) by_town
  order by years