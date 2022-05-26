CREATE TABLE [dbo].[documenti] (
    [codice]   INT           NOT NULL,
    [titolo]   VARCHAR (255) NOT NULL,
    [anno]     VARCHAR (4)   NULL,
    [settore]  VARCHAR (255) NOT NULL,
    [stato]    VARCHAR (11)  NOT NULL,
    [tipo]     VARCHAR (5)   NOT NULL,
    [scaffale] VARCHAR (10)  NOT NULL,
    PRIMARY KEY CLUSTERED ([codice] ASC),
    CHECK ([stato] = 'in prestito'
           OR [stato] = 'disponibile'),
    CHECK ([tipo] = 'dvd'
           OR [tipo] = 'libro'),
    CONSTRAINT [scaffale_key] FOREIGN KEY ([scaffale]) REFERENCES [dbo].[scaffali] ([codice])
);

