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
- WPF 기본학습
    - 데이터바인딩 마무리 
    - 디자인 리소스
- WPF MVVM

