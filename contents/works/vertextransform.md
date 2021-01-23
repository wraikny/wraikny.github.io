---
layout: post
title: Vertex Transform Shader
published: 2019-3-21
tags: work,vrchat,shader
---

適用したメッシュの描画時の位置・大きさ・角度を変更できるシェーダです。


Unityでのメッシュのレンダリングのための情報は主にShaderにより決定され、基本的にはvertex shaderによってオブジェクト座標系からワールド座標系、View座標系、Clipping座標系へと座標変換を行って3次元空間内での位置が決定され、fragment shaderによって描画する色が決定されます。本Shaderでは通常の行列変換に加えて、Materialで指定したPropertyをもとにvertex shader内でPosition(平行移動)、Scale(拡大)、Rotation(回転)による変換を与えることで、メッシュのTransformを柔軟に制御可能にしました。

<!--more-->

ライティングにはCubed Shaderを利用しています。

BOOTHにて販売しています。

[VertexTransformShader - wraikny's shop](https://wraikny.booth.pm/items/1222332)

ソースコード

[VertexTransformShader - GitHub](https://github.com/wraikny/VertexTransformShader)
