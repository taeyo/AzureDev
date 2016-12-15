### Azure Media Service 기반 Dynamic DRM Token 생성하는 예제
 - AES/PlayReady 기반의 DRM이 걸려있는 Asset에 대해서 자동으로 SWT, JWT 토큰을 만드는 예제
    - JWT는 Symmetric만 구현, X509 Cert는 구현하지 않음.
    - AES, PlayReady, Widevine 을 대상으로 SWT와 JWT(Symmetric)은 테스트됨.
 - [Azure Media Explorer](https://github.com/Azure/Azure-Media-Services-Explorer)의 소스를 참고했음.

