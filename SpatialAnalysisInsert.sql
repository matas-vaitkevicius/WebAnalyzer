--select  *,
--        GEOGRAPHY::STGeomFromText('POINT('+ 
--             SUBSTRING([Address], LEN(SUBSTRING([Address], 0, LEN([Address]) - CHARINDEX (',', [Address]))) + 1, 
--    LEN([Address]) - LEN(SUBSTRING([Address], 0, LEN([Address]) - CHARINDEX (',', [Address]))) - LEN(SUBSTRING(
--    col, CHARINDEX ('.', col), LEN(col))));convert(nvarchar(20), Longitude)+' '+
--            convert( nvarchar(20), Latitude)+')', 4326)
--        .STBuffer(Radius * 1000).STIntersects(@p) as [Intersects]

DECLARE  @i int = (SELECT min(id)   FROM [WebAnalyzer].[dbo].[Sale] s   where not exists(select 1 from SpatialAnalysis sa where sa.SaleId = s.Id))
--print @i
declare @temp table(
[Id] [int] IDENTITY(1,1)  NOT NULL,
	[SaleId] [int] NULL,
	--[RentId] [int] NULL,
	[Point] [geography] NULL,
	[RentsIn1kRadiusCount] [int] NULL,
	[SalesIn1kRadiusCount] [int] NULL,
	[RentsIn1kRadiusAvgSqM] [decimal](12, 6) NULL,
	[SalesIn1kRadiusAvgSqM] [decimal](12, 6) NULL,
	[RentsIn500RadiusCount] [int] NULL,
	[SalesIn500RadiusCount] [int] NULL,
	[RentsIn500RadiusAvgSqM] [decimal](12, 6) NULL,
	[SalesIn500RadiusAvgSqM] [decimal](12, 6) NULL,
	[RentsIn200RadiusCount] [int] NULL,
	[SalesIn200RadiusCount] [int] NULL,
	[RentsIn200RadiusAvgSqM] [decimal](12, 6) NULL,
	[SalesIn200RadiusAvgSqM] [decimal](12, 6) NULL,
	[RentsIn100RadiusCount] [int] NULL,
	[SalesIn100RadiusCount] [int] NULL,
	[RentsIn100RadiusAvgSqM] [decimal](12, 6) NULL,
	[SalesIn100RadiusAvgSqM] [decimal](12, 6) NULL
)

declare @lat nvarchar(20)
declare @lon nvarchar(20)
declare @rooms int
while @i <= (select max(id) from Webanalyzer.dbo.sale)
begin

 select top 1 
 @lat = SUBSTRING(Address,0,CHARINDEX(',',Address,0)), 
 @lon = SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)),
 @rooms = RoomCount from Webanalyzer.dbo.sale
		where id  = @i; 



declare @p GEOGRAPHY =  GEOGRAPHY::STGeomFromText('POINT('+ @lat +' '+   @lon        +')', 4326)


-------sales
		
	insert into  --@temp 
	 Webanalyzer.dbo.SpatialAnalysis
	([SaleId],[Point],[SalesIn1kRadiusCount],[SalesIn1kRadiusAvgSqM],[SalesIn500RadiusCount],[SalesIn200RadiusCount],[SalesIn100RadiusCount],
	[SalesIn500RadiusAvgSqM],[SalesIn200RadiusAvgSqM],[SalesIn100RadiusAvgSqM]
	)
select @i,  @p, prox.cnt,   prox.avg_sq_m,prox500.cnt, prox200.cnt, prox100.cnt,prox500.avg_sq_m,prox200.avg_sq_m,prox100.avg_sq_m   from
	(select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
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



------rents

update t set
 RentsIn1kRadiusCount = prox1k.cnt, [RentsIn1kRadiusAvgSqM] =		   prox1k.avg_sq_m,
 [RentsIn500RadiusCount] = prox.cnt, [RentsIn500RadiusAvgSqM] =		   prox.avg_sq_m  ,
[RentsIn200RadiusCount] = prox1.cnt, [RentsIn200RadiusAvgSqM] = prox1.avg_sq_m,
[RentsIn100RadiusCount] = prox100.cnt, [RentsIn100RadiusAvgSqM] = prox100.avg_sq_m
from 
	(select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  *, GEOGRAPHY::STGeomFromText('POINT('+ 
            convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
            convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326)
				.STBuffer(1000).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.rent
				  where --(Title = 'gandia' or Title = 'daimus')   and
 address is not null and LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox1k,
	(select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  *, GEOGRAPHY::STGeomFromText('POINT('+ 
            convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
            convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326)
				.STBuffer(500).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.rent
				  where --(Title = 'gandia' or Title = 'daimus')   and
 address is not null and LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox, (select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  *, GEOGRAPHY::STGeomFromText('POINT('+ 
            convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
            convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326)
				.STBuffer(200).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.rent
				  where --(Title = 'gandia' or Title = 'daimus')   and
 address is not null and LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox1,(select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  *, GEOGRAPHY::STGeomFromText('POINT('+ 
            convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
            convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326)
				.STBuffer(100).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.rent
				  where --(Title = 'gandia' or Title = 'daimus')   and
 address is not null and LivingArea is not null and RoomCount is not null and RoomCount = @rooms
			) s
		where [Intersects] = 1) prox100, 
		Webanalyzer.dbo.SpatialAnalysis t
		--@temp t
		where prox.id = t.Saleid and prox1.id = t.Saleid and prox100.id = t.Saleid and prox1k.id = t.Saleid
		--inner join Webanalyzer.dbo.sale r on  prox.id = r.id
	--	group by s.Id
		set @i = @i+1
		--select 
		--SUBSTRING(Address,0,CHARINDEX(',',Address,0)) lat, SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)) lon
	end	

	select * from @temp
	where [SalesIn1kRadiusCount] > 1
	-- order by ratio