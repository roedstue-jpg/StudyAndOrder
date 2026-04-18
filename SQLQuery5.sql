SELECT COUNT(*) AS StudiesCount FROM dbo.Studies;
SELECT COUNT(*) AS OrdersCount FROM dbo.Orders;
SELECT COUNT(*) AS IngoingMaterialsCount FROM dbo.IngoingMaterials;
SELECT COUNT(*) AS MaterialsCount FROM dbo.Materials;
SELECT COUNT(*) AS EquipmentsCount FROM dbo.Equipments;
SELECT TOP 10 * 
FROM dbo.__EFMigrationsHistory
ORDER BY MigrationId DESC;
USE SOMS_Db;
GO

INSERT INTO dbo.Materials (MaterialNumber, Description)
SELECT 'BA234572', N'Test material BA234572'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Materials WHERE MaterialNumber = 'BA234572');

INSERT INTO dbo.Materials (MaterialNumber, Description)
SELECT 'BS35672', N'Test material BS35672'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Materials WHERE MaterialNumber = 'BS35672');

-- evt. en ekstra
INSERT INTO dbo.Materials (MaterialNumber, Description)
SELECT 'CM000001', N'Test material CM000001'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Materials WHERE MaterialNumber = 'CM000001');
GO

INSERT INTO dbo.Equipments (EquipmentId, Description)
SELECT 'EQ-100', N'Test equipment EQ-100'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Equipments WHERE EquipmentId = 'EQ-100');

INSERT INTO dbo.Equipments (EquipmentId, Description)
SELECT 'EQ-200', N'Test equipment EQ-200'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Equipments WHERE EquipmentId = 'EQ-200');
GO

USE SOMS_Db;
GO
SELECT COUNT(*) AS MaterialsCount FROM dbo.Materials;
SELECT COUNT(*) AS EquipmentsCount FROM dbo.Equipments;