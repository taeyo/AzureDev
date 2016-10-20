# Azure 관련 개발소스들

1. [Fast Blob Uploader(Azure Storage)](AzureStorageUpload/) 
    - Azure Storage Data Movement Library (0.2.0) 사용
    - Console, WinForm 예제
    - 로컬 테스트 시에 AzCopy보다 나은 성능을 보임.
    - 참고링크 [Microsoft Azure Storage Data Movement Library (0.2.0)](https://github.com/Azure/azure-storage-net-data-movement)
2. [Azure Table WAD Viewer](AzTableDemo)
    - Azure Table에 있는 WAD 관련 테이블들의 로그를 가볍게 보는 프로그램
    - 물론, Azure Storage Explorer에서도 볼 수 있지만 그냥 만들어 봄.
3. [Azure SQL Database: Vertically Scale](AzureSQLDBVerticallyScale)
    - Azure SQL Database의 스케일을 변경할 수 있는 PowerShell Workflow Runbook
    - Azure Aumomation을 사용해서 특정 시간에 실행되도록 하거나 반복되게 할 수 있음
    - 새벽 시간에는 낮은 Tier로 구동하고, 낮 시간에는 높은 Tier로 구동하게 하고싶을 경우 등에 사용 가능