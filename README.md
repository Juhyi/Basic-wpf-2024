# Basic-wpf-2024
IoT 개발자 WPF 학습리포지토리

## 1일차
- WPF(Window Pressentation Foundation) 기본학습
    - Winforms 확장한 WPF
        - 이전의 Winforms는 이미지 비트맵방식(2D)
        - WPF 이미지 벡터방식
        - XAML 화면 디자인 - 안드로이드 개발시 Jaca XML로 화면디자인과 PyQt로 디자인과 동일
    
    - XAML(엑스에이엠엘, 재물)
        - 여는 태그 <Window>, 닫는 태그</Window>
        - 합치면 <Window /> - Window 태그 안에 다른객체가 없다는 뜻
        - 여는 태그와 닫는 태그 사이에 다른 태그(객체)를 넣어서 디자인

    - WPF 기본 사용법
        - Winforms와는 다르게 코딩으로 디자인함.

    - 레이아웃
        1. Grid - WPF에서 가장 많이 사용하는 대표적인 레이아웃
        2. StackPanel - 스택으로 컨트롤을 쌓는 레이아웃
        3. DockPanel - 컨트롤ㄹ을 방향에 따라서 도킹시키는 레이아웃
        4. Canvase - 미술에서 캔버스와 유사
        5. Margin - 여백기능, 앵커링 같이함(중요!)
    
## 2일차
- WPF 기본학습
    - 데이터 바인딩 - 데이터소스(DB, 엑셀, txt, 클라우드에 보관된 데이터원본)에 데이터를 쉽게 가져다쓰기 위해 데이터 핸들링방법
        - xaml : {Binding Path=속성, ElementName=객체, Mode=(OneWay|TwoWay), StringFormat={}{0:#,#}}
        - dataContext : 데이터를 담아서 전달하는 이름
        - 전통적인 읜폼 코드비하인드에서 데이터를 처리하는 것을 지양 - 디자인, 개발 부분 분리

## 3일차
- WPF에서 중요한 개념(윈폼과의 차이점)
    1. 데이터바인딩 - 바인딩 키워드로 코드와 분리
    2. 옵저버패턴 - 값이 변경된 사실을 사용자에게 공지 OnPropertyChanged 이벤트
    3. 디자인 리소스 - 각 컨트롤마다 디자인(X), 리소스로 디자인 공유 
        - 각 화면당 Resources - 자기 화면에만 적용되는 디자인
        - App.xaml Resources - 애플리케이션 전체에 적용되는 디자인
        - 리소스사전 - 공유 디자인 내용이 많을때 파일로 따로 지정

- WPF MVVM
    - MVC(Model View Controller 패턴)
        - 웹개발(Spring, ASP.NET MVC, dJango, etc...) 현재도 사용되고 있음
        - Model : Data 입출력 처리를 담당, 뷰에 제공할 데이터...
        - View : 디스플레이 화면 담당, 순수 xaml로만 구성 
        - Controller : View를 제어, Model 처리 중앙에 관장
    
    - MVVM(Model View ViewModel)
        - Model : Data 입출력(DB side), 뷰에 제공할 데이터...
        - View : 화면, 순수 xaml로만 구성
        - ViewModel : 뷰에 대한 메서드, 액션, INotifyPropertyChanged를 구현

        ![MVVM패턴](https://raw.githubusercontent.com/Juhyi/basic-wpf-2024/main/image/wpf001.png)

    - 권장 구현방법
        - ViewModel 생성, 알림 속성 구현,
        - View에 ViewModel을 데이터바인딩
        - Model DB작업 독립적으로 구현
    
    - MVVM 구현 도와주는 프레임워크
        0. ~~Mvvmlight.Toolkit~~ - 3rd Party 개발. 2009년부터 시작 2014년도 이후 더 이상 개발이나 지원이 없음
        1. **Caliburn.Micro** - 3rd Party 개발. MVVM이 아주 간단. 강력. 중소형 프로젝트에 적합 디버깅이 조금 어려움
        2. Avalonia - rd Party 개발. 크로스플랫폼, 디자인은 최고
        3. Prism - Microsoft 개발. 무지막지하게 어렵다. 대규모 프로젝트 활용
    
- Caliburn.Micro
    1. 프로젝트 생성 후 MainWindow.xaml 삭제
    2. Models, Views, ViewModels 폴더(네임스페이스) 생성
    3. 종속성 NudGet패키지 Carliburn.Micro 설치
    4. 루트 폴더에 Bootstrapper.cs 클래스 생성, 작성
    5. App.xaml에서 StartupUri 삭제
    6. App.xaml에 Bootstrapper 클래스를 리소스사전에 등록
    7. App.xaml.cs에 App() 생성자 추가
    8. ViewModels 폴더에 MainViewModel.cs 클래스 생성
    9. Bootstrapper.cs에 OnStartUp() 내용을 변경
    10. Views 폴더에 MainView.xaml 생성

    -  작업(3명) 분리
        - DB개발자 - DBMS 테이블 생성, Models에 클래스 작업
        - Xaml디자이너 - Views 폴더에 있는 xaml 파일을 디자인 작업
    
    ## 4일차
    - Cliburn.Micro
        - 작업분리
            - Xaml디자이너 - Xaml 파일만 디자인
            - ViewModel개발자 - Model에 있는 DB관련정보와 View와 연계 전체구현 작업

        - Cliburn.Micro 특징
            - Xaml 디자인시 {Binding ...} 잘 사용하지 않음
            - 대신 x:Name을 사용

        - MVVM 특징
            - 예외발생 시 예외메시지 표시없이 프로그램 종료
            - ViewModel에서 디버깅 시작
            - View.xaml 바인딩, 버튼클릭 이름(ViewModel 속성, 메서드) 지정 주의
            - Model은 DB 테이블 컬럼 이름 일치, CRUD 쿼리문 오타 주의
            - ViewModel 부분
                - 변수, 속성으로 분리
                - 속성이 Model내의 속성과 이름이 일치
                - List 사용불가 -> BindableCollection으로 변경
                - 메서드와 이름이 동일한 Can... 프로퍼티 지정, 버튼 활성/비활성화
                - 모든 속성에 NotifyOfPropertyChange() 메서드 존재!! (값 변경 알림)



    ![실행화면](https://raw.githubusercontent.com/Juhyi/basic-wpf-2024/main/image/wpf002.png)

## 5일차
- MahApps.Metro
    - Metro(Modern UI) 디자인 접목
    
    ![실행화면](https://raw.githubusercontent.com/Juhyi/basic-wpf-2024/main/image/wpf003.png)
    
    ![실행화면](https://raw.githubusercontent.com/Juhyi/basic-wpf-2024/main/image/wpf004.png)

- Movie API 연동 앱, MovieFinder 2024
    - 좋아하는 영화 즐겨찾기 앱
    - DB(SQL Server) 연동
    - MahApps.Metro UI
    - Cefsharp WebBrowser 패키지
    - Cefsharp.Apis 패키지
    - OpenAPI 두가지 사용
    - MVVM은 시간부족으로 사용안함
    - [TMDB](https://www.themoviedb.org/) 활용
        -  회원가입 후 API 신청
    - [Youtube API](https://console.cloud.google.com/) 활용
        - 새 프로젝트 생성
        - API 및 서비스, 라이브러리 선택
        - YouTube Data API v3 선택, 사용버튼 클릭
        - 사용자 인증정보 만들기 클릭
            1. 사용자 데이터 라디오버튼 클릭, 다음
            2. OAutho 동의하면, 기본내용 입력 후 다음
            3. 범위는 저장후 계속
            4. OAuth client ID, 앱유형을 데스크톱앱, 이름 입력 후 만들기 클릭

## 6일차
- MovieFinder 2024 남은것
    - 즐겨찾기 후 다시 선택 즐겨찾기 막아야함
    - 즐겨찾기 삭제 구현
    - 즐겨찾기 일부만 저장기능 추가
    - 그리드뷰 영화를 더블클릭하면 영화소개 팝업

## 7일차
- MovieFinder 2024 완료

  




- 데이터포털 API 연동앱 예제
    - 5.13 개인프로젝트 참조 소스

## 8일차
- WPF 개인프로젝트 포트폴리오 작업
