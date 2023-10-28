DO $$ 
BEGIN
IF NOT EXISTS (SELECT table_name FROM  information_schema.tables where table_name = LOWER('Cars') AND table_schema = 'public') THEN
    CREATE TABLE Cars (
    CarId SERIAL PRIMARY KEY,
    ModelName CHARACTER VARYING(255),
    Type CHARACTER VARYING(255),
    Doors Int,
    Status Int,
    Description CHARACTER VARYING(600),
    Image Text,
    Mileage Float,
    Brand CHARACTER VARYING(255),
    EngineSize Int,
    ModelYear Date,
    NextServedDate Date,
    LastServedDate Date
    );

END IF;
END $$;