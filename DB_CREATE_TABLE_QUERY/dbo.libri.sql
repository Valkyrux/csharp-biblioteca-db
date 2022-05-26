CREATE TABLE [dbo].[libri] (
    [codice]        INT NOT NULL,
    [numero_pagine] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([codice] ASC),
    CONSTRAINT [ref_documento_libro] FOREIGN KEY ([codice]) REFERENCES [dbo].[documenti] ([codice])
);

