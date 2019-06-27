/****** Script for SelectTopNRows command from SSMS  ******/
--select roomcount , avg(livingarea), count(1) from Rent where url like '%daft%'
--and LivingArea is not null
--group by RoomCount

--SELECT count(1)
--  FROM [WebAnalyzer].[dbo].[Rent] where url like '%daft%'

--  (SELECT count(1)   FROM [WebAnalyzer].[dbo].rent s   where   not exists(select 1 from SpatialAnalysis sa where sa.RentId = s.Id ) and address is not null)


  --insert into SpatialAnalysis (rentid,Point)
  --  select id,  GEOGRAPHY::STGeomFromText('POINT('+ 
  --          convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
  --          convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326) from rent where Address is not null and url like '%fotocasa%'
 -- delete
 --select * from SpatialAnalysis sa  
	--where
 -- --url like '%daft%' 
 -- sa.SalesIn1kRadiusCount is null
 -- ---and title = ''
--  delete  SpatialAnalysis where saleid in (
--  '7826',
--'8121',
--'8132',
--'8655',
--'13837',
--'15105',
--'15106',
--'16708',
--'17924')
--declare @temp table(
--[Id] [int] ) 

--insert into @temp

--SELECT sa.id, Count(*) 
--FROM [dbo].[SpatialAnalysis] sa
--JOIN [dbo].[SpatialAnalysis] around
--ON sa.Point.STBuffer(1000).STIntersects(around.Point) = 1
--GROUP BY sa.id
select adds.Url, adds.Price/adds.LivingArea Sqm, (adds.Price/adds.LivingArea)/k1.sale1Avg ratio , * from
(select 
baseid,  count(k1Rent.rentid) rent1kCount,  sum(k1Rent.RperSqM)/(count(k1Rent.rentid)) as rent1kAvgSqM,  count(around1k.SaleId) sale1kCount, (sum(k1sale.price)/sum(k1Sale.LivingArea)) sale1Avg,
(sum(k1sale.price)/sum(k1Sale.LivingArea))/((sum(k1Rent.RperSqM)/(count(k1Rent.rentid)))*12) years
--*
from 
(select  sa.id baseid, s.id saleid,s.RoomCount, point from webanalyzer.dbo.SpatialAnalysis sa inner join webanalyzer.dbo.Sale s on s.Id = SaleId where sa.SalesIn1kRadiusCount is null) as base
join webanalyzer.dbo.SpatialAnalysis around1k on base.Point.STBuffer(1000).STIntersects(around1k.Point) = 1 
left outer join (
 select id rentid, rc, Price/avgRoomSize RperSqM from( select * from (select rc, sum(avgArea*c)/sum(c) avgRoomSize from (select roomcount rc , avg(livingarea) avgArea, count(1) c from webanalyzer.dbo.Rent where url like '%daft%' and LivingArea is not null
group by RoomCount
union (select roomcount rc , avg(livingarea) avgArea, count(1) c from webanalyzer.dbo.sale where url like '%daft%' and LivingArea is not null
group by RoomCount)  )uni group by rc) avgRoom) avgrents
join webanalyzer.dbo.rent r on r.RoomCount = avgrents.rc
) k1Rent on k1Rent.rentid =around1k.RentId and base.RoomCount = k1Rent.rc
left outer join webanalyzer.dbo.Sale k1Sale on k1Sale.Id = around1k.SaleId and base.RoomCount = k1Sale.RoomCount

group by baseid) k1 
left outer join webanalyzer.dbo.SpatialAnalysis sp on sp.Id = baseid
left outer join WebAnalyzer.dbo.Sale adds on adds.Id = sp.SaleId
where adds.Price < 100000
order by  years, ratio

