---
layout: post
title: Do.fs
published: 2022-10-11
tags: library,fsharp
---

`InlineIfLambda`属性を利用して最適化された、F#のコンピューテーション式のビルダーの実装です。  
MITライセンスで単一ファイルで実装しています。
以下の型のビルダーを提供しています。

* `Option<'t>` 
* `ValueOption<'t>` 
* `Result<'t, 'e>` 
* `Lazy<'t>` 

[wraikny/Do.fs - GitHub](https://github.com/wraikny/Do.fs)

<!--more-->

### Examples

```fsharp
Do.voption {
  let! x = Some 40
  let! y = ValueSome 2
  return x + y
}
|> function
| ValueNone -> printfn "result is none."
| ValueSome a -> printfn "result = %d." a
```

### Reference

Gnicoさんのこちらの記事が参考になりました、ありがとうございます。  

[InlineIfLambdaを用いて効率の良いコンピュテーション式ビルダを作成する by Gnico - Zenn](https://zenn.dev/gnico/articles/5f133dac0585f5)
