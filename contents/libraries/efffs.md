---
layout: post
title: EffFs
published: 2021-4-9
tags: library,fsharp
---

EffFsは、SRTP を用いた F# の Effect System ライブラリです。

SideEffect や Dependency を静的に解決することができます。

- [wraikny/EffFs - GitHub](https://github.com/wraikny/EffFs)
- [EffFs - NuGet Gallery](https://www.nuget.org/packages/EffFs/)

<!--more-->

### Example

```fsharp
open EffFs

type RandomInt = RandomInt of int with
  static member Effect(_) = Eff.marker<int>

type Logging = Logging of string with
  static member Effect(_) = Eff.marker<unit>

let inline foo() = eff {
  let! a = RandomInt 100
  do! Logging (sprintf "%d" a)
  let b = a + a
  return (a, b)
}

let rand = System.Random()

type Handler = Handler with
  static member inline Value(_, x) = x

  static member inline Handle(RandomInt a, k) =
    rand.Next(a) |> k

  static member inline Handle(Logging s, k) =
    printfn "[Log] %s" s; k()

foo()
|> Eff.handle Handler
|> printfn "%A"


// example output
(*
[Log] 66
(66, 132)
*)
```