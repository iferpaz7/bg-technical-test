USE BgTechnicalTest 
GO
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ivan Paz
-- Create date: 2024-12-03
-- Description:	sp for get documents of customers
-- =============================================
CREATE OR ALTER PROCEDURE Config.Person_Get @userId INT
,@filters VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @idCard VARCHAR(25),
		@pageIndex INT,
		@pageSize INT;

	
	        SELECT @pageSize                 = pageSize
		          ,@pageIndex                = pageIndex
	        FROM OPENJSON(@filters) WITH 
            (
                pageIndex              INT 
		        ,pageSize               INT 
		     ) 
	BEGIN TRY

		SELECT p.[Id]
			,p.[FirstName]
			,p.[LastName]
			,p.[IdCard]
			,p.[Email]
			,p.[IdentificationTypeId]
			,it.[Name] identificationType
			,p.[Code]
			,p.[FullName]
			,p.[CreatedAt]
			,p.[Enabled]
			,COUNT(*) OVER() TotalRecords
		FROM Config.Person p WITH(NOLOCK)
			INNER JOIN Config.IdentificationType it WITH(NOLOCK) ON p.IdentificationTypeId = it.Id
		WHERE p.Deleted IS NULL
		ORDER BY Id DESC 
		OFFSET @pageSize*(@pageIndex-1) ROWS 
		FETCH NEXT @pageSize ROWS ONLY
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
GO
