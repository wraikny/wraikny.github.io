---
layout: post
title: EffFs
published: 2021-4-9
tags: work,library
---

EffFsは、SRTP を用いた F# の Effect System ライブラリです。

SideEffect や Dependency を静的に解決することができます。

- [EffFs - GitHub](https://github.com/wraikny/EffFs)
- [EffFs - NuGet Gallery](https://www.nuget.org/packages/EffFs/)

<!--more-->

Example

<div style='margin: 0px; padding: 0px; border: 1px solid #ececec; font-family: Monaco, Menlo, Consolas, monospace;'><style type='text/css'>.fs-str {color: #d14;} .fs-key {color: blue;} .fs-com {color: green; font-style: italic;}</style><table><tr><td style='padding: 5px; vertical-align: top; background-color: #ececec; color: rgb(160, 160, 160); font-size: 15px;'><span>1</span><br /><span>2</span><br /><span>3</span><br /><span>4</span><br /><span>5</span><br /><span>6</span><br /><span>7</span><br /><span>8</span><br /><span>9</span><br /><span>10</span><br /><span>11</span><br /><span>12</span><br /><span>13</span><br /><span>14</span><br /><span>15</span><br /><span>16</span><br /><span>17</span><br /><span>18</span><br /><span>19</span><br /><span>20</span><br /><span>21</span><br /><span>22</span><br /><span>23</span><br /><span>24</span><br /><span>25</span><br /><span>26</span><br /><span>27</span><br /><span>28</span><br /><span>29</span><br /><span>30</span><br /><span>31</span><br /><span>32</span><br /><span>33</span><br /><span>34</span><br /><span>35</span><br /><span>36</span><br /><span>37</span></td><td style='font-size: 15px; vertical-align: top; padding: 5px;'><pre style='margin: 0px; border: none; padding: 0; white-space: pre; font-size: 15px; background-color: white; font-family: Monaco, Menlo, Consolas, monospace;'><span class='fs-key'>open </span>EffFs
<span />
[&lt;Struct; NoEquality; NoComparison&gt;]
<span class='fs-key'>type </span>RandomInt = RandomInt <span class='fs-key'>of </span>int <span class='fs-key'>with
</span>  <span class='fs-key'>static </span><span class='fs-key'>member </span>Effect = Eff.marker&lt;int&gt;
<span />
[&lt;Struct; NoEquality; NoComparison&gt;]
<span class='fs-key'>type </span>Logging = Logging <span class='fs-key'>of </span>string <span class='fs-key'>with
</span>  <span class='fs-key'>static </span><span class='fs-key'>member </span>Effect = Eff.marker&lt;unit&gt;

<span class='fs-key'>let </span><span class='fs-key'>inline </span>foo() = eff {
<span />  <span class='fs-key'>let! </span>a = RandomInt 100
<span />  do! Logging (sprintf <span class='fs-str'>"%d"</span> a)
<span />  <span class='fs-key'>let </span>b = a + a
<span />  <span class='fs-key'>return </span>(a, b)
}

<span class='fs-key'>type </span>Handler = { rand: System.Random } <span class='fs-key'>with
</span>  <span class='fs-key'>static </span><span class='fs-key'>member </span><span class='fs-key'>inline </span>Handle(x) = x

<span />  <span class='fs-key'>static </span><span class='fs-key'>member </span><span class='fs-key'>inline </span>Handle(RandomInt a, k) =
<span />    Eff.capture(<span class='fs-key'>fun </span>h -&gt; h.rand.Next(a) |&gt; k)

<span />  <span class='fs-key'>static </span><span class='fs-key'>member </span><span class='fs-key'>inline </span>Handle(Logging s, k) =
<span />    printfn <span class='fs-str'>"[Log] %s"</span> s; k()

<span class='fs-key'>let </span>handler = { rand = System.Random() }
foo()
|&gt; Eff.handle handler
|&gt; printfn <span class='fs-str'>"%A"</span>


<span class='fs-com'>// example output</span>
<span class='fs-com'>(*
[Log] 66
(66, 132)
*)</span></pre></td></tr></table><div style='font-weight: bold; padding: 10px;'>Created with <a href='http://fslight.apphb.com/' target='_blank'>FsLight</a></div></div>
