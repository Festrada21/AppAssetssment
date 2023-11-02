USE master
GO

DROP DATABASE IF EXISTS dbapp;
CREATE DATABASE dbapp
GO

USE dbapp;
GO

DROP TABLE IF EXISTS Clientes;
-- Tabla de Clientes
CREATE TABLE Clientes (
    ClienteID INT PRIMARY KEY IDENTITY(1, 1) ,
    Nombre NVARCHAR(255),
    Codigo NVARCHAR(50),
    FechaAlta DATE,
    Direccion NVARCHAR(255),
    Correo NVARCHAR(255),
    Telefono NVARCHAR(20),
    EsClienteCable BIT,
    EsClienteInternet BIT
);

DROP TABLE IF EXISTS Servicios;
-- Tabla de Servicios
CREATE TABLE Servicios (
    ServicioID INT PRIMARY KEY IDENTITY(1, 1),
    ClienteID INT,
    TipoServicio NVARCHAR(50), -- Cable o Internet
    Velocidad INT, -- Para clientes de Internet
    TipoCable NVARCHAR(50), -- Básico o Premium para clientes de cable
    Ubicacion NVARCHAR(255),
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);

DROP TABLE IF EXISTS ServiciosSuspendidos;
CREATE TABLE ServiciosSuspendidos (
    SuspendidoID INT PRIMARY KEY,
    ClienteID INT,
    FechaSuspension DATE,
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);

DROP TABLE IF EXISTS PaquetesServicios;
-- Tabla de Paquetes de Servicios
CREATE TABLE PaquetesServicios (
    PaqueteID INT PRIMARY KEY IDENTITY(1, 1),
    Nombre NVARCHAR(255),
    CostoMensual DECIMAL(10, 2),
    Descuento DECIMAL(5, 2),
    AumentoVelocidad INT,
    UbicacionServicio NVARCHAR(255)
);

DROP TABLE IF EXISTS Facturas;
-- Tabla de Facturas
CREATE TABLE Facturas (
    FacturaID INT PRIMARY KEY IDENTITY(1, 1),
    ClienteID INT,
    NumeroFactura NVARCHAR(50),
    Fecha DATE,
    Serie NVARCHAR(20),
    Total DECIMAL(10, 2),
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);

DROP TABLE IF EXISTS DetallesFactura;
-- Tabla de Detalles de Factura
CREATE TABLE DetallesFactura (
    DetalleID INT PRIMARY KEY IDENTITY(1, 1),
    FacturaID INT,
    CodigoServicio NVARCHAR(20),
    TipoServicio NVARCHAR(50),
    CostoMensual DECIMAL(10, 2),
    Descuento DECIMAL(5, 2),
    CostoConDescuento DECIMAL(10, 2),
    FOREIGN KEY (FacturaID) REFERENCES Facturas(FacturaID)
);

DROP TABLE IF EXISTS Pagos;
-- Tabla de Pagos
CREATE TABLE Pagos (
    PagoID INT PRIMARY KEY IDENTITY(1, 1),
    FacturaID INT,
    FechaPago DATE,
    MontoPagado DECIMAL(10, 2),
    FOREIGN KEY (FacturaID) REFERENCES Facturas(FacturaID)
);

DROP TABLE IF EXISTS Empleados;
-- Tabla de Empleados
CREATE TABLE Empleados (
    EmpleadoID INT PRIMARY KEY IDENTITY(1, 1),
    NombreEmpleado NVARCHAR(255),
    Puesto NVARCHAR(50),
    Sueldo DECIMAL(10, 2),
    -- Otros campos relacionados con la nómina
);

DROP TABLE IF EXISTS PagosEmpleados;
-- Tabla de Pagos de Empleados
CREATE TABLE PagosEmpleados (
    PagoEmpleadoID INT PRIMARY KEY IDENTITY(1, 1),
    EmpleadoID INT,
    FechaPago DATE,
    MontoPagar DECIMAL(10, 2),
    CuotasIGSS DECIMAL(10, 2),
    RetencionISR DECIMAL(10, 2),
    FOREIGN KEY (EmpleadoID) REFERENCES Empleados(EmpleadoID)
);

USE dbapp;
-- Insertar----
INSERT INTO Clientes(Nombre,Codigo,FechaAlta,Direccion,Correo,Telefono,EsClienteCable,EsClienteInternet)
VALUES
  ('Octavia Landry',1,'08/08/2023','853-3178 Morbi Ave','nunc.mauris@google.couk','761404732',1,1),
  ('Noel Carroll',2,'08/12/2024','528-2763 Duis St.','adipiscing.mauris@icloud.edu','945334534',1,0),
  ('Sheila Lee',3,'09/24/2024','758-4256 Erat. Rd.','mauris.aliquam@outlook.ca','706886232',1,1),
  ('Neve Higgins',4,'01/28/2023','346-8187 Ipsum Av.','rutrum.justo@icloud.ca','258056325',1,1),
  ('Baker Joyner',5,'08/18/2024','1523 Lacinia Rd.','nunc.ullamcorper@yahoo.com','484810720',1,0);

INSERT INTO Servicios (ClienteID, TipoServicio, Velocidad, TipoCable, Ubicacion)
VALUES
    (1, 'Internet', 100, NULL, 'Ubicacion1'),
    (2, 'Cable', NULL, 'Básico', 'Ubicacion2'),
    (3, 'Internet', 50, NULL, 'Ubicacion3'),
    (4, 'Cable', NULL, 'Premium', 'Ubicacion4');


INSERT INTO ServiciosSuspendidos (SuspendidoID, ClienteID, FechaSuspension)
VALUES
    (1, 1, '2023-01-15'),
    (2, 3, '2023-02-10');

INSERT INTO PaquetesServicios (Nombre, CostoMensual, Descuento, AumentoVelocidad, UbicacionServicio)
VALUES
    ('Paquete1', 50.00, 5.00, 20, 'Ubicacion1'),
    ('Paquete2', 75.00, 7.50, 10, 'Ubicacion2');

INSERT INTO Facturas (ClienteID, NumeroFactura, Fecha, Serie, Total)
VALUES
    (1, 'F2023001', '2023-03-01', 'A', 50.00),
    (2, 'F2023002', '2023-03-02', 'B', 75.00);

INSERT INTO DetallesFactura (FacturaID, CodigoServicio, TipoServicio, CostoMensual, Descuento, CostoConDescuento)
VALUES
    (1, 'SVC1', 'Internet', 50.00, 5.00, 45.00),
    (2, 'SVC2', 'Cable', 75.00, 7.50, 67.50);

INSERT INTO Pagos (FacturaID, FechaPago, MontoPagado)
VALUES
    (1, '2023-03-05', 45.00),
    (2, '2023-03-06', 67.50);

INSERT INTO Empleados (NombreEmpleado, Puesto, Sueldo)
VALUES
    ('Empleado1', 'Técnico', 3000.00),
    ('Empleado2', 'Atención al Cliente', 2500.00);

INSERT INTO PagosEmpleados (EmpleadoID, FechaPago, MontoPagar, CuotasIGSS, RetencionISR)
VALUES
    (1, '2023-03-10', 3000.00, 150.00, 450.00),
    (2, '2023-03-11', 2500.00, 125.00, 375.00);

-- Procedimiento almacenado para asignar servicios contratados a un cliente
DROP PROCEDURE IF EXISTS AsignarServiciosACliente;

CREATE PROCEDURE AsignarServiciosACliente
    @ClienteID INT,
    @TipoServicio NVARCHAR(50),
    @Velocidad INT,
    @TipoCable NVARCHAR(50),
    @Ubicacion NVARCHAR(50)
AS
BEGIN
    -- Verificar si el cliente ya existe
    IF NOT EXISTS (SELECT 1 FROM Clientes WHERE ClienteID = @ClienteID)
    BEGIN
        THROW 51000, 'El cliente especificado no existe.', 1;
    END

    -- Insertar el servicio contratado
    INSERT INTO Servicios (ClienteID, TipoServicio, Velocidad, TipoCable, Ubicacion)
    VALUES (@ClienteID, @TipoServicio, @Velocidad, @TipoCable, @Ubicacion);
END

DROP PROCEDURE IF EXISTS EditarServicio;
-- Procedimiento almacenado para editar un servicio
CREATE PROCEDURE EditarServicio
    @ServicioID INT,
    @ClienteID INT,
    @TipoServicio NVARCHAR(50),
    @Velocidad INT,
    @TipoCable NVARCHAR(50),
    @Ubicacion NVARCHAR(255)
AS
BEGIN
    -- Verificar si el servicio especificado existe
    IF NOT EXISTS (SELECT 1 FROM Servicios WHERE ServicioID = @ServicioID)
    BEGIN
        THROW 51000, 'El servicio especificado no existe.', 1;
    END

    -- Actualizar el servicio con los nuevos valores
    UPDATE Servicios
    SET ClienteID = @ClienteID,
        TipoServicio = @TipoServicio,
        Velocidad = @Velocidad,
        TipoCable = @TipoCable,
        Ubicacion = @Ubicacion
    WHERE ServicioID = @ServicioID;
END
