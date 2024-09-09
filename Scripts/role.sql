CREATE TABLE "Role" ( 
    "Id" INT PRIMARY KEY, 
    "Name" VARCHAR(255) NOT NULL, 
    "Status" VARCHAR(50) NOT NULL DEFAULT 'Active',
    "CreatedAt" TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP, 
    "UpdatedAt" TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP 
);