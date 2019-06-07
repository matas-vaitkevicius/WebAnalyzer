--select  *,
--        GEOGRAPHY::STGeomFromText('POINT('+ 
--             SUBSTRING([Address], LEN(SUBSTRING([Address], 0, LEN([Address]) - CHARINDEX (',', [Address]))) + 1, 
--    LEN([Address]) - LEN(SUBSTRING([Address], 0, LEN([Address]) - CHARINDEX (',', [Address]))) - LEN(SUBSTRING(
--    col, CHARINDEX ('.', col), LEN(col))));convert(nvarchar(20), Longitude)+' '+
--            convert( nvarchar(20), Latitude)+')', 4326)
--        .STBuffer(Radius * 1000).STIntersects(@p) as [Intersects]

DECLARE  @i int = (SELECT   min(id)   FROM [WebAnalyzer].[dbo].[sale] s   where  url  like '%fotocasa%'  and not exists (select 1 from SpatialAnalysis where s.Id = SaleId))
declare @temp table(
r_sq_m decimal(12,5) ,
r_ratio decimal(12,5) ,
r_avg_sq_m decimal(12,5) ,
r_cnt int ,
 id int , 
url nvarchar(500) ,
title nvarchar(200),
addr nvarchar(50) ,
price decimal(12,5),
area decimal(12,5),
rooms int,
dateadded datetime 
)


while @i <=9901 --SELECT   max(id)   FROM [WebAnalyzer].[dbo].[sale]
begin
declare @lat nvarchar(20)
 select top 1 @lat = SUBSTRING(Address,0,CHARINDEX(',',Address,0)) from Webanalyzer.dbo.sale
		where id  = @i; 
declare @lon nvarchar(20)
 select top 1 @lon = SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)) from Webanalyzer.dbo.sale
		where id  = @i 


declare @p GEOGRAPHY =  GEOGRAPHY::STGeomFromText('POINT('+ @lat +' '+   @lon        +')', 4326)
		
	insert into	@temp
select price/LivingArea sq_m, (price/LivingArea)/avg_sq_m ratio, avg_sq_m, cnt, r.id, url, title, Address, price, LivingArea, RoomCount, DateAdded  from
	(select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  *, GEOGRAPHY::STGeomFromText('POINT('+ 
            convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
            convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326)
				.STBuffer(500).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.sale
				  where (Title = 'gandia' or Title = 'daimus')
  and address is not null and LivingArea is not null and RoomCount is not null
			) s
		where [Intersects] = 1) prox
		inner join Webanalyzer.dbo.sale r on  prox.id = r.id
	--	group by s.Id
		set @i = @i+1
		--select 
		--SUBSTRING(Address,0,CHARINDEX(',',Address,0)) lat, SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)) lon
	end	

	select * from @temp
	where r_cnt > 1
	 order by r_ratio