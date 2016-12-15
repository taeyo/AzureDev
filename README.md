# Azure 관련 개발소스들

1. [Fast Blob Uploader(Azure Storage) : WinForm](AzureStorageUpload/) 
    - Azure Storage Data Movement Library (0.2.0) 사용
    - Console, WinForm 예제
    - 로컬 테스트 시에 AzCopy보다 나은 성능을 보임.
    - 참고링크 [Microsoft Azure Storage Data Movement Library (0.2.0)](https://github.com/Azure/azure-storage-net-data-movement)
2. [Azure 파일 업로드 예제 : WebForm, HTML & Ajax](https://github.com/jiyongseong/AzurePaaSHol/tree/master/azure_storage_account/AzureFileUploadWeb)
    - ASP.NET MVC에서 그리드는 Grid.MVC를 활용
    - 추가 예제는 jQuery.Form을 활용한 HTML/Javascript 파일 업로드 방식으로 작성
    - 그리드는 Knockout을 활용하여 MVVM 으로 구현(Json 바인딩)
    - 서버 측은 Java나 Php 등으로 구현해도 무방함(예에서는 서버로 ASP.NET을 활용함)
    - 웹 페이지 혹은 스크립트를 통해서 업로드 되는 파일은 스트림 그대로 Azure Storage로 전송되도록 구현
    - 예제 소스는 이해하기 쉽도록 동기(Sync) 메서드를 사용하여 구현하였음
3. [Azure Table WAD Viewer](AzTableDemo)
    - Azure Table에 있는 WAD 관련 테이블들의 로그를 가볍게 보는 프로그램
    - 물론, Azure Storage Explorer에서도 볼 수 있지만 그냥 만들어 봄.
4. [Azure SQL Database: Vertically Scale](AzureSQLDBVerticallyScale)
    - Azure SQL Database의 스케일을 변경할 수 있는 PowerShell Workflow Runbook
    - Azure Aumomation을 사용해서 특정 시간에 실행되도록 하거나 반복되게 할 수 있음
    - 새벽 시간에는 낮은 Tier로 구동하고, 낮 시간에는 높은 Tier로 구동하게 하고싶을 경우 등에 사용 가능
5. [Azure Media Service에서 Dynamic DRM Token 생성하는 예제](AzureMS-Key)
    - AES/PlayReady 기반의 DRM이 걸려있는 Asset에 대해서 자동으로 SWT, JWT 토큰을 만드는 예제
    - JWT는 Symmetric만 구현, X509 Cert는 구현하지 않음.
    - 데모 소스이므로 단지 흐름을 참고하기 위한 용도로 활용하는 것이 좋음.
    - 더 깊은 내용은 [Azure Media Explorer](https://github.com/Azure/Azure-Media-Services-Explorer)의 소스를 참고바람
    