CREATE TABLE [dbo].[clienti] (
    [id]           INT           IDENTITY (0, 1) NOT NULL,
    [nome_cliente] VARCHAR (100) NOT NULL,
    [email]        VARCHAR (150) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

