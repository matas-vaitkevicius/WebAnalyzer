--select  *,
--        GEOGRAPHY::STGeomFromText('POINT('+ 
--             SUBSTRING([Address], LEN(SUBSTRING([Address], 0, LEN([Address]) - CHARINDEX (',', [Address]))) + 1, 
--    LEN([Address]) - LEN(SUBSTRING([Address], 0, LEN([Address]) - CHARINDEX (',', [Address]))) - LEN(SUBSTRING(
--    col, CHARINDEX ('.', col), LEN(col))));convert(nvarchar(20), Longitude)+' '+
--            convert( nvarchar(20), Latitude)+')', 4326)
--        .STBuffer(Radius * 1000).STIntersects(@p) as [Intersects]

Declare @temp table(id int IDENTITY(1,1),
saleid int ,
point GEOGRAPHY,
roomcount int
)

insert into @temp
SELECT saleid, point, roomcount  FROM [WebAnalyzer].[dbo].[Sale] s left outer join 
 [WebAnalyzer].[dbo].SpatialAnalysis sa on sa.SaleId = s.Id  
  where   url  like '%fotocasa%' 
  and SalesIn1kRadiusCount is null
  and point is not null
  -- and DateLastProcessed > '2019-06-27'
  and RoomCount is not null
  order by saleid


 DECLARE  @id int= 0-- (SELECT min(id)   FROM [WebAnalyzer].[dbo].[Sale] s   where   url  like '%fotocasa%'  and DateLastProcessed > '2019-06-27'
--and not exists(select 1 from [WebAnalyzer].[dbo].SpatialAnalysis sa where sa.SaleId = s.Id ))
--print @i
--declare @temp table(
--[Id] [int] IDENTITY(1,1)  NOT NULL,
--	[SaleId] [int] NULL,
--	--[RentId] [int] NULL,
--	[Point] [geography] NULL,
--	[RentsIn1kRadiusCount] [int] NULL,
--	[SalesIn1kRadiusCount] [int] NULL,
--	[RentsIn1kRadiusAvgSqM] [decimal](12, 6) NULL,
--	[SalesIn1kRadiusAvgSqM] [decimal](12, 6) NULL,
--	[RentsIn500RadiusCount] [int] NULL,
--	[SalesIn500RadiusCount] [int] NULL,
--	[RentsIn500RadiusAvgSqM] [decimal](12, 6) NULL,
--	[SalesIn500RadiusAvgSqM] [decimal](12, 6) NULL,
--	[RentsIn200RadiusCount] [int] NULL,
--	[SalesIn200RadiusCount] [int] NULL,
--	[RentsIn200RadiusAvgSqM] [decimal](12, 6) NULL,
--	[SalesIn200RadiusAvgSqM] [decimal](12, 6) NULL,
--	[RentsIn100RadiusCount] [int] NULL,
--	[SalesIn100RadiusCount] [int] NULL,
--	[RentsIn100RadiusAvgSqM] [decimal](12, 6) NULL, 
--	[SalesIn100RadiusAvgSqM] [decimal](12, 6) NULL
--)

--declare @lat nvarchar(20)
--declare @lon nvarchar(20)
DECLARE  @i int
declare @rooms int
declare @p GEOGRAPHY 
while @id <=   (select max(id) from @temp)
begin

 select top 1 
 --@lat = SUBSTRING(Address,0,CHARINDEX(',',Address,0)), 
 --@lon = SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)),
 @i = saleid,
 @p = Point,
 @rooms = RoomCount from @temp
 
		where id  = @id; 
	--	print @id 
	--	print  @i 
	--	print @rooms

	--	print '--------'
	--	set @id = @id+1
	--	--select 
	--	--SUBSTRING(Address,0,CHARINDEX(',',Address,0)) lat, SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)) lon
	--end	

--=  GEOGRAPHY::STGeomFromText('POINT('+ @lat +' '+   @lon        +')', 4326)


-------sales
	--print @i +' '+@rooms+' '+@p.Lat+' '+@p.Long
		
	update t set
 [SalesIn1kRadiusCount] = prox.cnt, [SalesIn1kRadiusAvgSqM] =		  prox.avg_sq_m,
 [SalesIn500RadiusCount] = prox500.cnt, [SalesIn500RadiusAvgSqM] =		   prox500.avg_sq_m  ,
[SalesIn200RadiusCount] = prox200.cnt, [SalesIn200RadiusAvgSqM] = prox200.avg_sq_m,
[SalesIn100RadiusCount] = prox100.cnt, [SalesIn100RadiusAvgSqM] = prox100.avg_sq_m,
 RentsIn1kRadiusCount = prox1k.cnt, [RentsIn1kRadiusAvgSqM] =		   prox1k.avg_sq_m,
 [RentsIn500RadiusCount] = prox5.cnt, [RentsIn500RadiusAvgSqM] =		   prox5.avg_sq_m  ,
[RentsIn200RadiusCount] = prox2.cnt, [RentsIn200RadiusAvgSqM] = prox2.avg_sq_m,
[RentsIn100RadiusCount] = prox1.cnt, [RentsIn100RadiusAvgSqM] = prox1.avg_sq_m
	from
	
	(select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  s.Price,s.LivingArea,sa.Point
				.STBuffer(1000).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.sale				s
				inner join  [WebAnalyzer].[dbo].SpatialAnalysis sa on  sa.SaleId = s.Id
				  where --(Title = 'gandia' or Title = 'daimus')   and
 --address is not null and 
 LivingArea is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox, 
		
		 (select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  s.Price,s.LivingArea,sa.Point
				.STBuffer(500).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.sale				s
				inner join  [WebAnalyzer].[dbo].SpatialAnalysis sa on  sa.SaleId = s.Id
				  where --(Title = 'gandia' or Title = 'daimus')   and
 --address is not null and 
 LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox500, 
		
		 (select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  s.Price,s.LivingArea, sa.Point
				.STBuffer(200).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.sale				s
				inner join  [WebAnalyzer].[dbo].SpatialAnalysis sa on  sa.SaleId = s.Id
				  where --(Title = 'gandia' or Title = 'daimus')   and
 --address is not null and 
 LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox200, 
		
		 (select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  s.Price,s.LivingArea,sa.Point
				.STBuffer(100).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.sale
				s
				inner join  [WebAnalyzer].[dbo].SpatialAnalysis sa on  sa.SaleId = s.Id
				  where --(Title = 'gandia' or Title = 'daimus')   and
 --address is not null and 
 LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox100 
		, (select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  r.Price,r.LivingArea, sa.Point
				.STBuffer(1000).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.rent
				r
				inner join  [WebAnalyzer].[dbo].SpatialAnalysis sa on  sa.RentId = r.Id
				  where --(Title = 'gandia' or Title = 'daimus')   and
 --address is not null and 
 LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox1k,
	(select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  r.Price,r.LivingArea,sa.Point
				.STBuffer(500).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.rent
				r
				inner join  [WebAnalyzer].[dbo].SpatialAnalysis sa on  sa.RentId = r.Id
				  where --(Title = 'gandia' or Title = 'daimus')   and
 --address is not null and 
 LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox5, (select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  r.Price,r.LivingArea, sa.Point
				.STBuffer(200).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.rent
				r
				inner join  [WebAnalyzer].[dbo].SpatialAnalysis sa on  sa.RentId = r.Id
				  where --(Title = 'gandia' or Title = 'daimus')   and
 --address is not null and 
 LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox2,(select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  r.Price,r.LivingArea, sa.Point
				.STBuffer(100).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.rent r
				inner join  [WebAnalyzer].[dbo].SpatialAnalysis sa on  sa.RentId = r.Id
				  where --(Title = 'gandia' or Title = 'daimus')   and
 --address is not null and 
 LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox1, 
		Webanalyzer.dbo.SpatialAnalysis t
		--@temp t
		where prox.id = t.Saleid and prox500.id = t.Saleid and prox100.id = t.Saleid and prox200.id = t.Saleid and prox5.id = t.Saleid and prox2.id = t.Saleid and prox1.id = t.Saleid and prox1k.id = t.Saleid



------rents


--from 
	
--		Webanalyzer.dbo.SpatialAnalysis t
--		--@temp t
--		where prox.id = t.Saleid and prox1.id = t.Saleid and prox100.id = t.Saleid and prox1k.id = t.Saleid
		--inner join Webanalyzer.dbo.sale r on  prox.id = r.id
	--	group by s.Id
		set @id = @id+1
		--select 
		--SUBSTRING(Address,0,CHARINDEX(',',Address,0)) lat, SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)) lon
	end	

	--select * from @temp
	--where [SalesIn1kRadiusCount] > 1
	-- order by ratio