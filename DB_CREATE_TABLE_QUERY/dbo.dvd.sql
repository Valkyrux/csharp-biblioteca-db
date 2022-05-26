CREATE TABLE [dbo].[dvd] (
    [codice] INT      NOT NULL,
    [durata] TIME (7) NOT NULL,
    PRIMARY KEY CLUSTERED ([codice] ASC),
    CONSTRAINT [ref_documento_dvd] FOREIGN KEY ([codice]) REFERENCES [dbo].[documenti] ([codice])
);

