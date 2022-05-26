CREATE TABLE [dbo].[scaffali] (
    [codice] VARCHAR (10)  NOT NULL,
    [sede]   VARCHAR (255) NULL,
    [stanza] VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([codice] ASC)
);

