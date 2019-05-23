--select  *,
--        GEOGRAPHY::STGeomFromText('POINT('+ 
--             SUBSTRING([Address], LEN(SUBSTRING([Address], 0, LEN([Address]) - CHARINDEX (',', [Address]))) + 1, 
--    LEN([Address]) - LEN(SUBSTRING([Address], 0, LEN([Address]) - CHARINDEX (',', [Address]))) - LEN(SUBSTRING(
--    col, CHARINDEX ('.', col), LEN(col))));convert(nvarchar(20), Longitude)+' '+
--            convert( nvarchar(20), Latitude)+')', 4326)
--        .STBuffer(Radius * 1000).STIntersects(@p) as [Intersects]

DECLARE  @i int = 993

while @i <=1000
begin
declare @lat nvarchar(20)
 select top 1 @lat = SUBSTRING(Address,0,CHARINDEX(',',Address,0)) from Webanalyzer.dbo.rent
		where id  = @i; 
declare @lon nvarchar(20)
 select top 1 @lon = SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)) from Webanalyzer.dbo.rent
		where id  = @i 


declare @p GEOGRAPHY =  GEOGRAPHY::STGeomFromText('POINT('+ @lat +' '+   @lon        +')', 4326)
		
		
select price/LivingArea sq_m, (price/LivingArea)/avg_sq_m, * from
	(select 	(sum(price)/sum(LivingArea)) avg_sq_m,  count(1) cnt, @i id from 
			(select  *, GEOGRAPHY::STGeomFromText('POINT('+ 
            convert(nvarchar(20), SUBSTRING(Address,0,CHARINDEX(',',Address,0)))+' '+
            convert( nvarchar(20), SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)))+')', 4326)
				.STBuffer(500).STIntersects(@p) as [Intersects]
				from Webanalyzer.dbo.rent
				where Address is not null 
			) s
		where [Intersects] = 1) prox
		inner join Webanalyzer.dbo.rent r on  prox.id = r.id
	--	group by s.Id
		set @i = @i+1
		--select 
		--SUBSTRING(Address,0,CHARINDEX(',',Address,0)) lat, SUBSTRING(Address,CHARINDEX(',',Address)+1,LEN(Address)) lon
	end	