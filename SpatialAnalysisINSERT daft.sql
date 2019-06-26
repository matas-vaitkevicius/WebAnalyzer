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
select * from
(select 
baseid, count(RentId) rent1kCount,  sum(k1Rent.Price)/(avgRentSize.avgArea*count(RentId)) as rent1kAvgSqM, avgRentSize.avgArea, count(around1k.SaleId) sale1kCount
--*
from 
(select  sa.id baseid, s.id saleid,s.RoomCount, point from SpatialAnalysis sa inner join Sale s on s.Id = SaleId where sa.SalesIn1kRadiusCount is null) as base
join SpatialAnalysis around1k on base.Point.STBuffer(1000).STIntersects(around1k.Point) = 1 
join Rent k1Rent on k1Rent.Id =around1k.RentId and base.RoomCount = k1Rent.RoomCount
join Sale k1Sale on k1Sale.Id = around1k.SaleId and base.RoomCount = k1Sale.RoomCount
join (select roomcount , avg(livingarea) avgArea, count(1) c from Rent where url like '%daft%' and LivingArea is not null
group by RoomCount) avgRentSize on avgRentSize.RoomCount = k1Rent.RoomCount
group by baseid, avgRentSize.avgArea) k1 



select   @p, prox.cnt,   prox.avg_sq_m,prox500.cnt, prox200.cnt, prox100.cnt,prox500.avg_sq_m,prox200.avg_sq_m,prox100.avg_sq_m   from
	(select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt from 
			(select  *, GEOGRAPHY::STGeomFromText('POINT('+ 
            convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
            convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326)
				.STBuffer(1000).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.sale
				  where --(Title = 'gandia' or Title = 'daimus')   and
 address is not null and LivingArea is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox, 
		
		 (select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  *, GEOGRAPHY::STGeomFromText('POINT('+ 
            convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
            convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326)
				.STBuffer(500).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.sale
				  where --(Title = 'gandia' or Title = 'daimus')   and
 address is not null and LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox500, 
		
		 (select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  *, GEOGRAPHY::STGeomFromText('POINT('+ 
            convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
            convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326)
				.STBuffer(200).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.sale
				  where --(Title = 'gandia' or Title = 'daimus')   and
 address is not null and LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox200, 
		
		 (select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  *, GEOGRAPHY::STGeomFromText('POINT('+ 
            convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
            convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326)
				.STBuffer(100).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.sale
				  where --(Title = 'gandia' or Title = 'daimus')   and
 address is not null and LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox100 where prox500.id = prox.id and prox200.id = prox.id and prox100.id = prox.id 
