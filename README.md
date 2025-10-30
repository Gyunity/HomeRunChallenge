# HomeRun Challenge ⚾
캐주얼/스포츠 모바일 야구 타격 게임 (Android)

[![Unity](https://img.shields.io/badge/Unity-6.x-black?logo=unity)]()
![Platform](https://img.shields.io/badge/Platform-Android-green)
![Status](https://img.shields.io/badge/Status-Playable-brightgreen)

> 변화구/직구를 **예측 + 타이밍**으로 정확히 때려 **홈런**을 노리는 1인 개발 프로젝트

---

## ✨ 핵심 요약
- **핵심 재미**: 타이밍/위치 정확도로 타구 각·속도가 달라지는 “손맛”
- **차별화**: 실제 타격 감각을 반영한 **사운드/궤도 예측/스윙 타이밍 연출**
- **진행**: 라운드가 오를수록 다양한 구종과 빠른 구속의 투수 등장

---

## 🎮 플레이 방법
| 항목 | 내용 |
|---|---|
| 조작 | 공이 타격 구역에 들어올 때 **터치** |
| 판정 | **Perfect(홈런)** / **Good(안타)** / **Miss(헛스윙)** |
| 투수 | 포심·투심·슬라이더·커브 / 투수별 고유 패턴 |
| 목표 | 라운드마다 정해진 투구 수 내에 **고득점** 달성 |

> 홈런 시 “HOMERUN!” 배너가 **슬라이드 인/아웃** + 진동 연출로 나타납니다.

---

## 📦 다운로드 & 실행
- **APK**: (예) `Releases` 탭에 업로드한 `HomeRunChallenge_v1.0.3.apk`  
- **소스 빌드**
  1) Unity 6.x로 열기  
  2) `File ▸ Build Settings ▸ Android` 전환  
  3) 메인 씬 추가 후 `Build and Run`

---

## 🧠 시스템 개요
- **GameFlowManager**: 라운드 루프/점수/전환 UI 제어
- **HitInputHandler**: 터치 → `TimingResult(Offset, Accuracy)` 산출
- **PositionJudge**: 동일 평면에서 `inputPoint ↔ expectedPoint` 거리 → **0~1 정확도**
- **HitPhysicsCalculator**: 정확도 → **출구 속도/수직각/수평각** 계산
- **BallController**: **구면 합성 방식**으로 발사 벡터 생성
- **BallRangeUtil**: **지면 높이**와의 교차로 착지 예측 → 홈런 판정
- **SlideBanner**: CanvasGroup + RectTransform 슬라이드 & 페이드 연출
- **VibrationManager**: Android **VibratorManager/VibrationEffect** 분기 (Unity 6)
  
<pre>
Assets/Scripts/
 ├─ Control/      BallController.cs, HitInputHandler.cs
 ├─ Judge/        TimingJudge.cs, PositionJudge.cs
 ├─ PhysicsCalc/  HitPhysicsCalculator.cs, PitchingAni.cs
 ├─ Trajectory/   CurvePitchTrajectory.cs, IPitchTrajectory.cs, TrajectoryPredictor.cs
 ├─ Manager/      GameFlowManager.cs, PitchingManager.cs, BattingManager.cs,
 │                ScoreManager.cs, HighScoreManager.cs, SoundManager.cs, EffectManager.cs,
 │                RoundConfig.cs, VibrationManager.cs
 ├─ UI/           RoundTransitionUI.cs, SlideBanner.cs, PauseMenu.cs
 └─ Visuals/      LandingVisualizer.cs, HitDistance.cs, EffectPreset_SO.cs, EffectType.cs
</pre>

---


## 🗺️ 로드맵
- [ ] 투수 AI 패턴 고도화(카운트/심리전)
- [ ] 랭킹/기록 공유
- [ ] 타구 사운드/파티클 **스윗스팟** 세분화
- [ ] 튜토리얼/초반 난이도 커브 다듬기

---

## 📝 패치노트
**1.0.1**: 파티클 오류 수정, 타격 진동, 투구 SFX, 가이드 라인 추가  
**1.0.2**: 타격 각도 조정, 애니메이션 오류 수정  
**1.0.3**: 일시정지 수정, 타자 애니 수정, **홈런 배너 추가**

---

## 📄 라이선스 & 연락
- 개인 포트폴리오·비상업적 공개  
- Dev: GYUNITY — ghl7276@naver.com
