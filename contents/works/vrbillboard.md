---
layout: post
title: VR Billboard Shader
published: 2019-3-20
tags: work,vrchat,shader
---

VR向けのBillboardライクなシェーダ(Unity)です。


通常のパーティクル等に利用されるビルボードシェーダの実装としては、座標変換の際にCameraのView座標系を基準にして頂点位置を指定することでカメラに水平方向にメッシュを貼ります。しかしVR内においては、その実装だと首を回した際にカメラの動きに追従してメッシュが回転する様子が露骨にわかってしまい、ゲーム内での体験を損なってしまいます。そこで本Shaderではオブジェクトから両眼の中点の方向に垂直な面にメッシュを貼ることで常にカメラの方向に向くようにしました。

<!--more-->

VR状態で頭を振ってもくるくると回転しません。
VRChatのネームタグと近い挙動をします。

BOOTHにて販売しています。

[VR Billboard Shader - wraikny's shop](https://wraikny.booth.pm/items/1091055)
