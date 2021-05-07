---
layout: post
title: Affogato
published: 2019-10-14
tags: library,fsharp
---

Affogato は F# でのゲームプログラミングを目的とした、SRTP によるベクトル型と、いくつかのアルゴリズムのためのライブラリです。

- [Affogato - GitHub](https://github.com/wraikny/Affogato)
- [Affogato - NuGet Gallery](https://www.nuget.org/packages/Affogato/)

Example

```fsharp
open Affogato
let v1 = Vector2.init 1 0 // int Vector2
let v2 = Vector2.init 2.1f 3.4f // float32 Vector2
let v3 = v1 |> Vector.map float32 // float32 Vector2
let v4 = v2 / v3 .* 2.0f * v2.yy + v3.xx // float32 Vector2
let r1 = Rectangle.init v3 v4 // float32 Vector2 Rectangle
let k = Vector.dot v2 v3 // float32
```
