



<!DOCTYPE html>
<html>
<head>
 <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" >
 <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" >
 
 <meta name="ROBOTS" content="NOARCHIVE">
 
 <link rel="icon" type="image/vnd.microsoft.icon" href="http://www.gstatic.com/codesite/ph/images/phosting.ico">
 
 
 <script type="text/javascript">
 
 
 
 
 var codesite_token = "Lw_q3elB2KxwGPLvKAovdZ9PG64:1351385300363";
 
 
 var CS_env = {"profileUrl":"/u/112436863544550044548/","token":"Lw_q3elB2KxwGPLvKAovdZ9PG64:1351385300363","assetHostPath":"http://www.gstatic.com/codesite/ph","domainName":null,"assetVersionPath":"http://www.gstatic.com/codesite/ph/4212538301465177006","projectHomeUrl":"/p/phpwcms","relativeBaseUrl":"","projectName":"phpwcms","loggedInUserEmail":"tripathi.smriti@gmail.com"};
 var _gaq = _gaq || [];
 _gaq.push(
 ['siteTracker._setAccount', 'UA-18071-1'],
 ['siteTracker._trackPageview']);
 
 _gaq.push(
 ['projectTracker._setAccount', 'UA-82903-16'],
 ['projectTracker._trackPageview']);
 
 (function() {
 var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
 ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
 (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(ga);
 })();
 
 </script>
 
 
 <title>bootstrap-tab.js - 
 phpwcms -
 
 
 A very flexible, fast, powerful, customer and developer friendly web content management system and cms framework - Google Project Hosting
 </title>
 <link type="text/css" rel="stylesheet" href="http://www.gstatic.com/codesite/ph/4212538301465177006/css/core.css">
 
 <link type="text/css" rel="stylesheet" href="http://www.gstatic.com/codesite/ph/4212538301465177006/css/ph_detail.css" >
 
 
 <link type="text/css" rel="stylesheet" href="http://www.gstatic.com/codesite/ph/4212538301465177006/css/d_sb.css" >
 
 
 
<!--[if IE]>
 <link type="text/css" rel="stylesheet" href="http://www.gstatic.com/codesite/ph/4212538301465177006/css/d_ie.css" >
<![endif]-->
 <style type="text/css">
 .menuIcon.off { background: no-repeat url(http://www.gstatic.com/codesite/ph/images/dropdown_sprite.gif) 0 -42px }
 .menuIcon.on { background: no-repeat url(http://www.gstatic.com/codesite/ph/images/dropdown_sprite.gif) 0 -28px }
 .menuIcon.down { background: no-repeat url(http://www.gstatic.com/codesite/ph/images/dropdown_sprite.gif) 0 0; }
 
 
 
  tr.inline_comment {
 background: #fff;
 vertical-align: top;
 }
 div.draft, div.published {
 padding: .3em;
 border: 1px solid #999; 
 margin-bottom: .1em;
 font-family: arial, sans-serif;
 max-width: 60em;
 }
 div.draft {
 background: #ffa;
 } 
 div.published {
 background: #e5ecf9;
 }
 div.published .body, div.draft .body {
 padding: .5em .1em .1em .1em;
 max-width: 60em;
 white-space: pre-wrap;
 white-space: -moz-pre-wrap;
 white-space: -pre-wrap;
 white-space: -o-pre-wrap;
 word-wrap: break-word;
 font-size: 1em;
 }
 div.draft .actions {
 margin-left: 1em;
 font-size: 90%;
 }
 div.draft form {
 padding: .5em .5em .5em 0;
 }
 div.draft textarea, div.published textarea {
 width: 95%;
 height: 10em;
 font-family: arial, sans-serif;
 margin-bottom: .5em;
 }

 
 .nocursor, .nocursor td, .cursor_hidden, .cursor_hidden td {
 background-color: white;
 height: 2px;
 }
 .cursor, .cursor td {
 background-color: darkblue;
 height: 2px;
 display: '';
 }
 
 
.list {
 border: 1px solid white;
 border-bottom: 0;
}

 
 </style>
</head>
<body class="t4">
<script type="text/javascript">
 window.___gcfg = {lang: 'en'};
 (function() 
 {var po = document.createElement("script");
 po.type = "text/javascript"; po.async = true;po.src = "https://apis.google.com/js/plusone.js";
 var s = document.getElementsByTagName("script")[0];
 s.parentNode.insertBefore(po, s);
 })();
</script>
<div class="headbg">

 <div id="gaia">
 

 <span>
 
 
 
 <a href="#" id="multilogin-dropdown" onclick="return false;"
 ><u><b>tripathi.smriti@gmail.com</b></u> <small>&#9660;</small></a>
 
 
 | <a href="/u/112436863544550044548/" id="projects-dropdown" onclick="return false;"
 ><u>My favorites</u> <small>&#9660;</small></a>
 | <a href="/u/112436863544550044548/" onclick="_CS_click('/gb/ph/profile');"
 title="Profile, Updates, and Settings"
 ><u>Profile</u></a>
 | <a href="https://www.google.com/accounts/Logout?continue=http%3A%2F%2Fcode.google.com%2Fp%2Fphpwcms%2Fsource%2Fbrowse%2Fbranches%2Fdev-2.0%2Finclude%2Fjs%2Fbootstrap-tab.js%3Fr%3D481" 
 onclick="_CS_click('/gb/ph/signout');"
 ><u>Sign out</u></a>
 
 </span>

 </div>

 <div class="gbh" style="left: 0pt;"></div>
 <div class="gbh" style="right: 0pt;"></div>
 
 
 <div style="height: 1px"></div>
<!--[if lte IE 7]>
<div style="text-align:center;">
Your version of Internet Explorer is not supported. Try a browser that
contributes to open source, such as <a href="http://www.firefox.com">Firefox</a>,
<a href="http://www.google.com/chrome">Google Chrome</a>, or
<a href="http://code.google.com/chrome/chromeframe/">Google Chrome Frame</a>.
</div>
<![endif]-->



 <table style="padding:0px; margin: 0px 0px 10px 0px; width:100%" cellpadding="0" cellspacing="0"
 itemscope itemtype="http://schema.org/CreativeWork">
 <tr style="height: 58px;">
 
 
 
 <td id="plogo">
 <link itemprop="url" href="/p/phpwcms">
 <a href="/p/phpwcms/">
 
 
 <img src="/p/phpwcms/logo?cct=1333993019"
 alt="Logo" itemprop="image">
 
 </a>
 </td>
 
 <td style="padding-left: 0.5em">
 
 <div id="pname">
 <a href="/p/phpwcms/"><span itemprop="name">phpwcms</span></a>
 </div>
 
 <div id="psum">
 <a id="project_summary_link"
 href="/p/phpwcms/"><span itemprop="description">A very flexible, fast, powerful, customer and developer friendly web content management system and cms framework</span></a>
 
 </div>
 
 
 </td>
 <td style="white-space:nowrap;text-align:right; vertical-align:bottom;">
 
 <form action="/hosting/search">
 <input size="30" name="q" value="" type="text">
 
 <input type="submit" name="projectsearch" value="Search projects" >
 </form>
 
 </tr>
 </table>

</div>

 
<div id="mt" class="gtb"> 
 <a href="/p/phpwcms/" class="tab ">Project&nbsp;Home</a>
 
 
 
 
 <a href="/p/phpwcms/downloads/list" class="tab ">Downloads</a>
 
 
 
 
 
 <a href="/p/phpwcms/w/list" class="tab ">Wiki</a>
 
 
 
 
 
 <a href="/p/phpwcms/issues/list"
 class="tab ">Issues</a>
 
 
 
 
 
 <a href="/p/phpwcms/source/checkout"
 class="tab active">Source</a>
 
 
 
 
 
 
 
 <div class=gtbc></div>
</div>
<table cellspacing="0" cellpadding="0" width="100%" align="center" border="0" class="st">
 <tr>
 
 
 
 
 
 
 
 <td class="subt">
 <div class="st2">
 <div class="isf">
 
 


 <span class="inst1"><a href="/p/phpwcms/source/checkout">Checkout</a></span> &nbsp;
 <span class="inst2"><a href="/p/phpwcms/source/browse/trunk">Browse</a></span> &nbsp;
 <span class="inst3"><a href="/p/phpwcms/source/list">Changes</a></span> &nbsp;
 
 &nbsp;
 
 
 <form action="/p/phpwcms/source/search" method="get" style="display:inline"
 onsubmit="document.getElementById('codesearchq').value = document.getElementById('origq').value">
 <input type="hidden" name="q" id="codesearchq" value="">
 <input type="text" maxlength="2048" size="38" id="origq" name="origq" value="" title="Google Code Search" style="font-size:92%">&nbsp;<input type="submit" value="Search Trunk" name="btnG" style="font-size:92%">
 
 
 </form>
 <script type="text/javascript">
 
 function codesearchQuery(form) {
 var query = document.getElementById('q').value;
 if (query) { form.action += '%20' + query; }
 }
 </script>
 </div>
</div>

 </td>
 
 
 
 <td align="right" valign="top" class="bevel-right"></td>
 </tr>
</table>


<script type="text/javascript">
 var cancelBubble = false;
 function _go(url) { document.location = url; }
</script>
<div id="maincol"
 
>

 




<div class="expand">
<div id="colcontrol">
<style type="text/css">
 #file_flipper { white-space: nowrap; padding-right: 2em; }
 #file_flipper.hidden { display: none; }
 #file_flipper .pagelink { color: #0000CC; text-decoration: underline; }
 #file_flipper #visiblefiles { padding-left: 0.5em; padding-right: 0.5em; }
</style>
<table id="nav_and_rev" class="list"
 cellpadding="0" cellspacing="0" width="100%">
 <tr>
 
 <td nowrap="nowrap" class="src_crumbs src_nav" width="33%">
 <strong class="src_nav">Source path:&nbsp;</strong>
 <span id="crumb_root">
 
 <a href="/p/phpwcms/source/browse/?r=481">svn</a>/&nbsp;</span>
 <span id="crumb_links" class="ifClosed"><a href="/p/phpwcms/source/browse/branches/?r=481">branches</a><span class="sp">/&nbsp;</span><a href="/p/phpwcms/source/browse/branches/dev-2.0/?r=481">dev-2.0</a><span class="sp">/&nbsp;</span><a href="/p/phpwcms/source/browse/branches/dev-2.0/include/?r=481">include</a><span class="sp">/&nbsp;</span><a href="/p/phpwcms/source/browse/branches/dev-2.0/include/js/?r=481">js</a><span class="sp">/&nbsp;</span>bootstrap-tab.js</span>
 
 


 </td>
 
 
 <td nowrap="nowrap" width="33%" align="right">
 <table cellpadding="0" cellspacing="0" style="font-size: 100%"><tr>
 
 
 <td class="flipper"><b>r481</b></td>
 
 </tr></table>
 </td> 
 </tr>
</table>

<div class="fc">
 
 
 
<style type="text/css">
.undermouse span {
 background-image: url(http://www.gstatic.com/codesite/ph/images/comments.gif); }
</style>
<table class="opened" id="review_comment_area"
><tr>
<td id="nums">
<pre><table width="100%"><tr class="nocursor"><td></td></tr></table></pre>
<pre><table width="100%" id="nums_table_0"><tr id="gr_svn481_1"

><td id="1"><a href="#1">1</a></td></tr
><tr id="gr_svn481_2"

><td id="2"><a href="#2">2</a></td></tr
><tr id="gr_svn481_3"

><td id="3"><a href="#3">3</a></td></tr
><tr id="gr_svn481_4"

><td id="4"><a href="#4">4</a></td></tr
><tr id="gr_svn481_5"

><td id="5"><a href="#5">5</a></td></tr
><tr id="gr_svn481_6"

><td id="6"><a href="#6">6</a></td></tr
><tr id="gr_svn481_7"

><td id="7"><a href="#7">7</a></td></tr
><tr id="gr_svn481_8"

><td id="8"><a href="#8">8</a></td></tr
><tr id="gr_svn481_9"

><td id="9"><a href="#9">9</a></td></tr
><tr id="gr_svn481_10"

><td id="10"><a href="#10">10</a></td></tr
><tr id="gr_svn481_11"

><td id="11"><a href="#11">11</a></td></tr
><tr id="gr_svn481_12"

><td id="12"><a href="#12">12</a></td></tr
><tr id="gr_svn481_13"

><td id="13"><a href="#13">13</a></td></tr
><tr id="gr_svn481_14"

><td id="14"><a href="#14">14</a></td></tr
><tr id="gr_svn481_15"

><td id="15"><a href="#15">15</a></td></tr
><tr id="gr_svn481_16"

><td id="16"><a href="#16">16</a></td></tr
><tr id="gr_svn481_17"

><td id="17"><a href="#17">17</a></td></tr
><tr id="gr_svn481_18"

><td id="18"><a href="#18">18</a></td></tr
><tr id="gr_svn481_19"

><td id="19"><a href="#19">19</a></td></tr
><tr id="gr_svn481_20"

><td id="20"><a href="#20">20</a></td></tr
><tr id="gr_svn481_21"

><td id="21"><a href="#21">21</a></td></tr
><tr id="gr_svn481_22"

><td id="22"><a href="#22">22</a></td></tr
><tr id="gr_svn481_23"

><td id="23"><a href="#23">23</a></td></tr
><tr id="gr_svn481_24"

><td id="24"><a href="#24">24</a></td></tr
><tr id="gr_svn481_25"

><td id="25"><a href="#25">25</a></td></tr
><tr id="gr_svn481_26"

><td id="26"><a href="#26">26</a></td></tr
><tr id="gr_svn481_27"

><td id="27"><a href="#27">27</a></td></tr
><tr id="gr_svn481_28"

><td id="28"><a href="#28">28</a></td></tr
><tr id="gr_svn481_29"

><td id="29"><a href="#29">29</a></td></tr
><tr id="gr_svn481_30"

><td id="30"><a href="#30">30</a></td></tr
><tr id="gr_svn481_31"

><td id="31"><a href="#31">31</a></td></tr
><tr id="gr_svn481_32"

><td id="32"><a href="#32">32</a></td></tr
><tr id="gr_svn481_33"

><td id="33"><a href="#33">33</a></td></tr
><tr id="gr_svn481_34"

><td id="34"><a href="#34">34</a></td></tr
><tr id="gr_svn481_35"

><td id="35"><a href="#35">35</a></td></tr
><tr id="gr_svn481_36"

><td id="36"><a href="#36">36</a></td></tr
><tr id="gr_svn481_37"

><td id="37"><a href="#37">37</a></td></tr
><tr id="gr_svn481_38"

><td id="38"><a href="#38">38</a></td></tr
><tr id="gr_svn481_39"

><td id="39"><a href="#39">39</a></td></tr
><tr id="gr_svn481_40"

><td id="40"><a href="#40">40</a></td></tr
><tr id="gr_svn481_41"

><td id="41"><a href="#41">41</a></td></tr
><tr id="gr_svn481_42"

><td id="42"><a href="#42">42</a></td></tr
><tr id="gr_svn481_43"

><td id="43"><a href="#43">43</a></td></tr
><tr id="gr_svn481_44"

><td id="44"><a href="#44">44</a></td></tr
><tr id="gr_svn481_45"

><td id="45"><a href="#45">45</a></td></tr
><tr id="gr_svn481_46"

><td id="46"><a href="#46">46</a></td></tr
><tr id="gr_svn481_47"

><td id="47"><a href="#47">47</a></td></tr
><tr id="gr_svn481_48"

><td id="48"><a href="#48">48</a></td></tr
><tr id="gr_svn481_49"

><td id="49"><a href="#49">49</a></td></tr
><tr id="gr_svn481_50"

><td id="50"><a href="#50">50</a></td></tr
><tr id="gr_svn481_51"

><td id="51"><a href="#51">51</a></td></tr
><tr id="gr_svn481_52"

><td id="52"><a href="#52">52</a></td></tr
><tr id="gr_svn481_53"

><td id="53"><a href="#53">53</a></td></tr
><tr id="gr_svn481_54"

><td id="54"><a href="#54">54</a></td></tr
><tr id="gr_svn481_55"

><td id="55"><a href="#55">55</a></td></tr
><tr id="gr_svn481_56"

><td id="56"><a href="#56">56</a></td></tr
><tr id="gr_svn481_57"

><td id="57"><a href="#57">57</a></td></tr
><tr id="gr_svn481_58"

><td id="58"><a href="#58">58</a></td></tr
><tr id="gr_svn481_59"

><td id="59"><a href="#59">59</a></td></tr
><tr id="gr_svn481_60"

><td id="60"><a href="#60">60</a></td></tr
><tr id="gr_svn481_61"

><td id="61"><a href="#61">61</a></td></tr
><tr id="gr_svn481_62"

><td id="62"><a href="#62">62</a></td></tr
><tr id="gr_svn481_63"

><td id="63"><a href="#63">63</a></td></tr
><tr id="gr_svn481_64"

><td id="64"><a href="#64">64</a></td></tr
><tr id="gr_svn481_65"

><td id="65"><a href="#65">65</a></td></tr
><tr id="gr_svn481_66"

><td id="66"><a href="#66">66</a></td></tr
><tr id="gr_svn481_67"

><td id="67"><a href="#67">67</a></td></tr
><tr id="gr_svn481_68"

><td id="68"><a href="#68">68</a></td></tr
><tr id="gr_svn481_69"

><td id="69"><a href="#69">69</a></td></tr
><tr id="gr_svn481_70"

><td id="70"><a href="#70">70</a></td></tr
><tr id="gr_svn481_71"

><td id="71"><a href="#71">71</a></td></tr
><tr id="gr_svn481_72"

><td id="72"><a href="#72">72</a></td></tr
><tr id="gr_svn481_73"

><td id="73"><a href="#73">73</a></td></tr
><tr id="gr_svn481_74"

><td id="74"><a href="#74">74</a></td></tr
><tr id="gr_svn481_75"

><td id="75"><a href="#75">75</a></td></tr
><tr id="gr_svn481_76"

><td id="76"><a href="#76">76</a></td></tr
><tr id="gr_svn481_77"

><td id="77"><a href="#77">77</a></td></tr
><tr id="gr_svn481_78"

><td id="78"><a href="#78">78</a></td></tr
><tr id="gr_svn481_79"

><td id="79"><a href="#79">79</a></td></tr
><tr id="gr_svn481_80"

><td id="80"><a href="#80">80</a></td></tr
><tr id="gr_svn481_81"

><td id="81"><a href="#81">81</a></td></tr
><tr id="gr_svn481_82"

><td id="82"><a href="#82">82</a></td></tr
><tr id="gr_svn481_83"

><td id="83"><a href="#83">83</a></td></tr
><tr id="gr_svn481_84"

><td id="84"><a href="#84">84</a></td></tr
><tr id="gr_svn481_85"

><td id="85"><a href="#85">85</a></td></tr
><tr id="gr_svn481_86"

><td id="86"><a href="#86">86</a></td></tr
><tr id="gr_svn481_87"

><td id="87"><a href="#87">87</a></td></tr
><tr id="gr_svn481_88"

><td id="88"><a href="#88">88</a></td></tr
><tr id="gr_svn481_89"

><td id="89"><a href="#89">89</a></td></tr
><tr id="gr_svn481_90"

><td id="90"><a href="#90">90</a></td></tr
><tr id="gr_svn481_91"

><td id="91"><a href="#91">91</a></td></tr
><tr id="gr_svn481_92"

><td id="92"><a href="#92">92</a></td></tr
><tr id="gr_svn481_93"

><td id="93"><a href="#93">93</a></td></tr
><tr id="gr_svn481_94"

><td id="94"><a href="#94">94</a></td></tr
><tr id="gr_svn481_95"

><td id="95"><a href="#95">95</a></td></tr
><tr id="gr_svn481_96"

><td id="96"><a href="#96">96</a></td></tr
><tr id="gr_svn481_97"

><td id="97"><a href="#97">97</a></td></tr
><tr id="gr_svn481_98"

><td id="98"><a href="#98">98</a></td></tr
><tr id="gr_svn481_99"

><td id="99"><a href="#99">99</a></td></tr
><tr id="gr_svn481_100"

><td id="100"><a href="#100">100</a></td></tr
><tr id="gr_svn481_101"

><td id="101"><a href="#101">101</a></td></tr
><tr id="gr_svn481_102"

><td id="102"><a href="#102">102</a></td></tr
><tr id="gr_svn481_103"

><td id="103"><a href="#103">103</a></td></tr
><tr id="gr_svn481_104"

><td id="104"><a href="#104">104</a></td></tr
><tr id="gr_svn481_105"

><td id="105"><a href="#105">105</a></td></tr
><tr id="gr_svn481_106"

><td id="106"><a href="#106">106</a></td></tr
><tr id="gr_svn481_107"

><td id="107"><a href="#107">107</a></td></tr
><tr id="gr_svn481_108"

><td id="108"><a href="#108">108</a></td></tr
><tr id="gr_svn481_109"

><td id="109"><a href="#109">109</a></td></tr
><tr id="gr_svn481_110"

><td id="110"><a href="#110">110</a></td></tr
><tr id="gr_svn481_111"

><td id="111"><a href="#111">111</a></td></tr
><tr id="gr_svn481_112"

><td id="112"><a href="#112">112</a></td></tr
><tr id="gr_svn481_113"

><td id="113"><a href="#113">113</a></td></tr
><tr id="gr_svn481_114"

><td id="114"><a href="#114">114</a></td></tr
><tr id="gr_svn481_115"

><td id="115"><a href="#115">115</a></td></tr
><tr id="gr_svn481_116"

><td id="116"><a href="#116">116</a></td></tr
><tr id="gr_svn481_117"

><td id="117"><a href="#117">117</a></td></tr
><tr id="gr_svn481_118"

><td id="118"><a href="#118">118</a></td></tr
><tr id="gr_svn481_119"

><td id="119"><a href="#119">119</a></td></tr
><tr id="gr_svn481_120"

><td id="120"><a href="#120">120</a></td></tr
><tr id="gr_svn481_121"

><td id="121"><a href="#121">121</a></td></tr
><tr id="gr_svn481_122"

><td id="122"><a href="#122">122</a></td></tr
><tr id="gr_svn481_123"

><td id="123"><a href="#123">123</a></td></tr
><tr id="gr_svn481_124"

><td id="124"><a href="#124">124</a></td></tr
><tr id="gr_svn481_125"

><td id="125"><a href="#125">125</a></td></tr
><tr id="gr_svn481_126"

><td id="126"><a href="#126">126</a></td></tr
><tr id="gr_svn481_127"

><td id="127"><a href="#127">127</a></td></tr
><tr id="gr_svn481_128"

><td id="128"><a href="#128">128</a></td></tr
><tr id="gr_svn481_129"

><td id="129"><a href="#129">129</a></td></tr
><tr id="gr_svn481_130"

><td id="130"><a href="#130">130</a></td></tr
><tr id="gr_svn481_131"

><td id="131"><a href="#131">131</a></td></tr
><tr id="gr_svn481_132"

><td id="132"><a href="#132">132</a></td></tr
><tr id="gr_svn481_133"

><td id="133"><a href="#133">133</a></td></tr
><tr id="gr_svn481_134"

><td id="134"><a href="#134">134</a></td></tr
><tr id="gr_svn481_135"

><td id="135"><a href="#135">135</a></td></tr
></table></pre>
<pre><table width="100%"><tr class="nocursor"><td></td></tr></table></pre>
</td>
<td id="lines">
<pre><table width="100%"><tr class="cursor_stop cursor_hidden"><td></td></tr></table></pre>
<pre class="prettyprint lang-js"><table id="src_table_0"><tr
id=sl_svn481_1

><td class="source">/* ========================================================<br></td></tr
><tr
id=sl_svn481_2

><td class="source"> * bootstrap-tab.js v2.0.3<br></td></tr
><tr
id=sl_svn481_3

><td class="source"> * http://twitter.github.com/bootstrap/javascript.html#tabs<br></td></tr
><tr
id=sl_svn481_4

><td class="source"> * ========================================================<br></td></tr
><tr
id=sl_svn481_5

><td class="source"> * Copyright 2012 Twitter, Inc.<br></td></tr
><tr
id=sl_svn481_6

><td class="source"> *<br></td></tr
><tr
id=sl_svn481_7

><td class="source"> * Licensed under the Apache License, Version 2.0 (the &quot;License&quot;);<br></td></tr
><tr
id=sl_svn481_8

><td class="source"> * you may not use this file except in compliance with the License.<br></td></tr
><tr
id=sl_svn481_9

><td class="source"> * You may obtain a copy of the License at<br></td></tr
><tr
id=sl_svn481_10

><td class="source"> *<br></td></tr
><tr
id=sl_svn481_11

><td class="source"> * http://www.apache.org/licenses/LICENSE-2.0<br></td></tr
><tr
id=sl_svn481_12

><td class="source"> *<br></td></tr
><tr
id=sl_svn481_13

><td class="source"> * Unless required by applicable law or agreed to in writing, software<br></td></tr
><tr
id=sl_svn481_14

><td class="source"> * distributed under the License is distributed on an &quot;AS IS&quot; BASIS,<br></td></tr
><tr
id=sl_svn481_15

><td class="source"> * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.<br></td></tr
><tr
id=sl_svn481_16

><td class="source"> * See the License for the specific language governing permissions and<br></td></tr
><tr
id=sl_svn481_17

><td class="source"> * limitations under the License.<br></td></tr
><tr
id=sl_svn481_18

><td class="source"> * ======================================================== */<br></td></tr
><tr
id=sl_svn481_19

><td class="source"><br></td></tr
><tr
id=sl_svn481_20

><td class="source"><br></td></tr
><tr
id=sl_svn481_21

><td class="source">!function ($) {<br></td></tr
><tr
id=sl_svn481_22

><td class="source"><br></td></tr
><tr
id=sl_svn481_23

><td class="source">  &quot;use strict&quot;; // jshint ;_;<br></td></tr
><tr
id=sl_svn481_24

><td class="source"><br></td></tr
><tr
id=sl_svn481_25

><td class="source"><br></td></tr
><tr
id=sl_svn481_26

><td class="source"> /* TAB CLASS DEFINITION<br></td></tr
><tr
id=sl_svn481_27

><td class="source">  * ==================== */<br></td></tr
><tr
id=sl_svn481_28

><td class="source"><br></td></tr
><tr
id=sl_svn481_29

><td class="source">  var Tab = function ( element ) {<br></td></tr
><tr
id=sl_svn481_30

><td class="source">    this.element = $(element)<br></td></tr
><tr
id=sl_svn481_31

><td class="source">  }<br></td></tr
><tr
id=sl_svn481_32

><td class="source"><br></td></tr
><tr
id=sl_svn481_33

><td class="source">  Tab.prototype = {<br></td></tr
><tr
id=sl_svn481_34

><td class="source"><br></td></tr
><tr
id=sl_svn481_35

><td class="source">    constructor: Tab<br></td></tr
><tr
id=sl_svn481_36

><td class="source"><br></td></tr
><tr
id=sl_svn481_37

><td class="source">  , show: function () {<br></td></tr
><tr
id=sl_svn481_38

><td class="source">      var $this = this.element<br></td></tr
><tr
id=sl_svn481_39

><td class="source">        , $ul = $this.closest(&#39;ul:not(.dropdown-menu)&#39;)<br></td></tr
><tr
id=sl_svn481_40

><td class="source">        , selector = $this.attr(&#39;data-target&#39;)<br></td></tr
><tr
id=sl_svn481_41

><td class="source">        , previous<br></td></tr
><tr
id=sl_svn481_42

><td class="source">        , $target<br></td></tr
><tr
id=sl_svn481_43

><td class="source">        , e<br></td></tr
><tr
id=sl_svn481_44

><td class="source"><br></td></tr
><tr
id=sl_svn481_45

><td class="source">      if (!selector) {<br></td></tr
><tr
id=sl_svn481_46

><td class="source">        selector = $this.attr(&#39;href&#39;)<br></td></tr
><tr
id=sl_svn481_47

><td class="source">        selector = selector &amp;&amp; selector.replace(/.*(?=#[^\s]*$)/, &#39;&#39;) //strip for ie7<br></td></tr
><tr
id=sl_svn481_48

><td class="source">      }<br></td></tr
><tr
id=sl_svn481_49

><td class="source"><br></td></tr
><tr
id=sl_svn481_50

><td class="source">      if ( $this.parent(&#39;li&#39;).hasClass(&#39;active&#39;) ) return<br></td></tr
><tr
id=sl_svn481_51

><td class="source"><br></td></tr
><tr
id=sl_svn481_52

><td class="source">      previous = $ul.find(&#39;.active a&#39;).last()[0]<br></td></tr
><tr
id=sl_svn481_53

><td class="source"><br></td></tr
><tr
id=sl_svn481_54

><td class="source">      e = $.Event(&#39;show&#39;, {<br></td></tr
><tr
id=sl_svn481_55

><td class="source">        relatedTarget: previous<br></td></tr
><tr
id=sl_svn481_56

><td class="source">      })<br></td></tr
><tr
id=sl_svn481_57

><td class="source"><br></td></tr
><tr
id=sl_svn481_58

><td class="source">      $this.trigger(e)<br></td></tr
><tr
id=sl_svn481_59

><td class="source"><br></td></tr
><tr
id=sl_svn481_60

><td class="source">      if (e.isDefaultPrevented()) return<br></td></tr
><tr
id=sl_svn481_61

><td class="source"><br></td></tr
><tr
id=sl_svn481_62

><td class="source">      $target = $(selector)<br></td></tr
><tr
id=sl_svn481_63

><td class="source"><br></td></tr
><tr
id=sl_svn481_64

><td class="source">      this.activate($this.parent(&#39;li&#39;), $ul)<br></td></tr
><tr
id=sl_svn481_65

><td class="source">      this.activate($target, $target.parent(), function () {<br></td></tr
><tr
id=sl_svn481_66

><td class="source">        $this.trigger({<br></td></tr
><tr
id=sl_svn481_67

><td class="source">          type: &#39;shown&#39;<br></td></tr
><tr
id=sl_svn481_68

><td class="source">        , relatedTarget: previous<br></td></tr
><tr
id=sl_svn481_69

><td class="source">        })<br></td></tr
><tr
id=sl_svn481_70

><td class="source">      })<br></td></tr
><tr
id=sl_svn481_71

><td class="source">    }<br></td></tr
><tr
id=sl_svn481_72

><td class="source"><br></td></tr
><tr
id=sl_svn481_73

><td class="source">  , activate: function ( element, container, callback) {<br></td></tr
><tr
id=sl_svn481_74

><td class="source">      var $active = container.find(&#39;&gt; .active&#39;)<br></td></tr
><tr
id=sl_svn481_75

><td class="source">        , transition = callback<br></td></tr
><tr
id=sl_svn481_76

><td class="source">            &amp;&amp; $.support.transition<br></td></tr
><tr
id=sl_svn481_77

><td class="source">            &amp;&amp; $active.hasClass(&#39;fade&#39;)<br></td></tr
><tr
id=sl_svn481_78

><td class="source"><br></td></tr
><tr
id=sl_svn481_79

><td class="source">      function next() {<br></td></tr
><tr
id=sl_svn481_80

><td class="source">        $active<br></td></tr
><tr
id=sl_svn481_81

><td class="source">          .removeClass(&#39;active&#39;)<br></td></tr
><tr
id=sl_svn481_82

><td class="source">          .find(&#39;&gt; .dropdown-menu &gt; .active&#39;)<br></td></tr
><tr
id=sl_svn481_83

><td class="source">          .removeClass(&#39;active&#39;)<br></td></tr
><tr
id=sl_svn481_84

><td class="source"><br></td></tr
><tr
id=sl_svn481_85

><td class="source">        element.addClass(&#39;active&#39;)<br></td></tr
><tr
id=sl_svn481_86

><td class="source"><br></td></tr
><tr
id=sl_svn481_87

><td class="source">        if (transition) {<br></td></tr
><tr
id=sl_svn481_88

><td class="source">          element[0].offsetWidth // reflow for transition<br></td></tr
><tr
id=sl_svn481_89

><td class="source">          element.addClass(&#39;in&#39;)<br></td></tr
><tr
id=sl_svn481_90

><td class="source">        } else {<br></td></tr
><tr
id=sl_svn481_91

><td class="source">          element.removeClass(&#39;fade&#39;)<br></td></tr
><tr
id=sl_svn481_92

><td class="source">        }<br></td></tr
><tr
id=sl_svn481_93

><td class="source"><br></td></tr
><tr
id=sl_svn481_94

><td class="source">        if ( element.parent(&#39;.dropdown-menu&#39;) ) {<br></td></tr
><tr
id=sl_svn481_95

><td class="source">          element.closest(&#39;li.dropdown&#39;).addClass(&#39;active&#39;)<br></td></tr
><tr
id=sl_svn481_96

><td class="source">        }<br></td></tr
><tr
id=sl_svn481_97

><td class="source"><br></td></tr
><tr
id=sl_svn481_98

><td class="source">        callback &amp;&amp; callback()<br></td></tr
><tr
id=sl_svn481_99

><td class="source">      }<br></td></tr
><tr
id=sl_svn481_100

><td class="source"><br></td></tr
><tr
id=sl_svn481_101

><td class="source">      transition ?<br></td></tr
><tr
id=sl_svn481_102

><td class="source">        $active.one($.support.transition.end, next) :<br></td></tr
><tr
id=sl_svn481_103

><td class="source">        next()<br></td></tr
><tr
id=sl_svn481_104

><td class="source"><br></td></tr
><tr
id=sl_svn481_105

><td class="source">      $active.removeClass(&#39;in&#39;)<br></td></tr
><tr
id=sl_svn481_106

><td class="source">    }<br></td></tr
><tr
id=sl_svn481_107

><td class="source">  }<br></td></tr
><tr
id=sl_svn481_108

><td class="source"><br></td></tr
><tr
id=sl_svn481_109

><td class="source"><br></td></tr
><tr
id=sl_svn481_110

><td class="source"> /* TAB PLUGIN DEFINITION<br></td></tr
><tr
id=sl_svn481_111

><td class="source">  * ===================== */<br></td></tr
><tr
id=sl_svn481_112

><td class="source"><br></td></tr
><tr
id=sl_svn481_113

><td class="source">  $.fn.tab = function ( option ) {<br></td></tr
><tr
id=sl_svn481_114

><td class="source">    return this.each(function () {<br></td></tr
><tr
id=sl_svn481_115

><td class="source">      var $this = $(this)<br></td></tr
><tr
id=sl_svn481_116

><td class="source">        , data = $this.data(&#39;tab&#39;)<br></td></tr
><tr
id=sl_svn481_117

><td class="source">      if (!data) $this.data(&#39;tab&#39;, (data = new Tab(this)))<br></td></tr
><tr
id=sl_svn481_118

><td class="source">      if (typeof option == &#39;string&#39;) data[option]()<br></td></tr
><tr
id=sl_svn481_119

><td class="source">    })<br></td></tr
><tr
id=sl_svn481_120

><td class="source">  }<br></td></tr
><tr
id=sl_svn481_121

><td class="source"><br></td></tr
><tr
id=sl_svn481_122

><td class="source">  $.fn.tab.Constructor = Tab<br></td></tr
><tr
id=sl_svn481_123

><td class="source"><br></td></tr
><tr
id=sl_svn481_124

><td class="source"><br></td></tr
><tr
id=sl_svn481_125

><td class="source"> /* TAB DATA-API<br></td></tr
><tr
id=sl_svn481_126

><td class="source">  * ============ */<br></td></tr
><tr
id=sl_svn481_127

><td class="source"><br></td></tr
><tr
id=sl_svn481_128

><td class="source">  $(function () {<br></td></tr
><tr
id=sl_svn481_129

><td class="source">    $(&#39;body&#39;).on(&#39;click.tab.data-api&#39;, &#39;[data-toggle=&quot;tab&quot;], [data-toggle=&quot;pill&quot;]&#39;, function (e) {<br></td></tr
><tr
id=sl_svn481_130

><td class="source">      e.preventDefault()<br></td></tr
><tr
id=sl_svn481_131

><td class="source">      $(this).tab(&#39;show&#39;)<br></td></tr
><tr
id=sl_svn481_132

><td class="source">    })<br></td></tr
><tr
id=sl_svn481_133

><td class="source">  })<br></td></tr
><tr
id=sl_svn481_134

><td class="source"><br></td></tr
><tr
id=sl_svn481_135

><td class="source">}(window.jQuery);<br></td></tr
></table></pre>
<pre><table width="100%"><tr class="cursor_stop cursor_hidden"><td></td></tr></table></pre>
</td>
</tr></table>

 
<script type="text/javascript">
 var lineNumUnderMouse = -1;
 
 function gutterOver(num) {
 gutterOut();
 var newTR = document.getElementById('gr_svn481_' + num);
 if (newTR) {
 newTR.className = 'undermouse';
 }
 lineNumUnderMouse = num;
 }
 function gutterOut() {
 if (lineNumUnderMouse != -1) {
 var oldTR = document.getElementById(
 'gr_svn481_' + lineNumUnderMouse);
 if (oldTR) {
 oldTR.className = '';
 }
 lineNumUnderMouse = -1;
 }
 }
 var numsGenState = {table_base_id: 'nums_table_'};
 var srcGenState = {table_base_id: 'src_table_'};
 var alignerRunning = false;
 var startOver = false;
 function setLineNumberHeights() {
 if (alignerRunning) {
 startOver = true;
 return;
 }
 numsGenState.chunk_id = 0;
 numsGenState.table = document.getElementById('nums_table_0');
 numsGenState.row_num = 0;
 if (!numsGenState.table) {
 return; // Silently exit if no file is present.
 }
 srcGenState.chunk_id = 0;
 srcGenState.table = document.getElementById('src_table_0');
 srcGenState.row_num = 0;
 alignerRunning = true;
 continueToSetLineNumberHeights();
 }
 function rowGenerator(genState) {
 if (genState.row_num < genState.table.rows.length) {
 var currentRow = genState.table.rows[genState.row_num];
 genState.row_num++;
 return currentRow;
 }
 var newTable = document.getElementById(
 genState.table_base_id + (genState.chunk_id + 1));
 if (newTable) {
 genState.chunk_id++;
 genState.row_num = 0;
 genState.table = newTable;
 return genState.table.rows[0];
 }
 return null;
 }
 var MAX_ROWS_PER_PASS = 1000;
 function continueToSetLineNumberHeights() {
 var rowsInThisPass = 0;
 var numRow = 1;
 var srcRow = 1;
 while (numRow && srcRow && rowsInThisPass < MAX_ROWS_PER_PASS) {
 numRow = rowGenerator(numsGenState);
 srcRow = rowGenerator(srcGenState);
 rowsInThisPass++;
 if (numRow && srcRow) {
 if (numRow.offsetHeight != srcRow.offsetHeight) {
 numRow.firstChild.style.height = srcRow.offsetHeight + 'px';
 }
 }
 }
 if (rowsInThisPass >= MAX_ROWS_PER_PASS) {
 setTimeout(continueToSetLineNumberHeights, 10);
 } else {
 alignerRunning = false;
 if (startOver) {
 startOver = false;
 setTimeout(setLineNumberHeights, 500);
 }
 }
 }
 function initLineNumberHeights() {
 // Do 2 complete passes, because there can be races
 // between this code and prettify.
 startOver = true;
 setTimeout(setLineNumberHeights, 250);
 window.onresize = setLineNumberHeights;
 }
 initLineNumberHeights();
</script>

 
 
 <div id="log">
 <div style="text-align:right">
 <a class="ifCollapse" href="#" onclick="_toggleMeta(this); return false">Show details</a>
 <a class="ifExpand" href="#" onclick="_toggleMeta(this); return false">Hide details</a>
 </div>
 <div class="ifExpand">
 
 
 <div class="pmeta_bubble_bg" style="border:1px solid white">
 <div class="round4"></div>
 <div class="round2"></div>
 <div class="round1"></div>
 <div class="box-inner">
 <div id="changelog">
 <p>Change log</p>
 <div>
 <a href="/p/phpwcms/source/detail?spec=svn481&amp;r=481">r481</a>
 by slackero
 on May 2, 2012
 &nbsp; <a href="/p/phpwcms/source/diff?spec=svn481&r=481&amp;format=side&amp;path=/branches/dev-2.0/include/js/bootstrap-tab.js&amp;old_path=/branches/dev-2.0/include/js/bootstrap-tab.js&amp;old=">Diff</a>
 </div>
 <pre>Prepare the new backend design</pre>
 </div>
 
 
 
 
 
 
 <script type="text/javascript">
 var detail_url = '/p/phpwcms/source/detail?r=481&spec=svn481';
 var publish_url = '/p/phpwcms/source/detail?r=481&spec=svn481#publish';
 // describe the paths of this revision in javascript.
 var changed_paths = [];
 var changed_urls = [];
 
 changed_paths.push('/branches/dev-2.0/COMPONENTS.md');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/COMPONENTS.md?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/GPL');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/GPL?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/LICENSE');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/LICENSE?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/README.md');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/README.md?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/css/colorpicker.css');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/css/colorpicker.css?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/css/datepicker.css');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/css/datepicker.css?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/css/phpwcms-v2-responsive.css');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/css/phpwcms-v2-responsive.css?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/css/phpwcms-v2.css');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/css/phpwcms-v2.css?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/img/alpha.png');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/img/alpha.png?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/img/glyphicons-halflings-white.png');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/img/glyphicons-halflings-white.png?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/img/glyphicons-halflings.png');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/img/glyphicons-halflings.png?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/img/hue.png');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/img/hue.png?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/img/phpwcms-signet-login.png');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/img/phpwcms-signet-login.png?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/img/saturation.png');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/img/saturation.png?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-alert.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-alert.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-button.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-button.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-carousel.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-carousel.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-collapse.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-collapse.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-colorpicker.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-colorpicker.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-datepicker.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-datepicker.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-dropdown.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-dropdown.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-modal.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-modal.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-popover.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-popover.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-scrollspy.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-scrollspy.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-tab.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-tab.js?r\x3d481\x26spec\x3dsvn481');
 
 var selected_path = '/branches/dev-2.0/include/js/bootstrap-tab.js';
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-tooltip.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-tooltip.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-transition.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-transition.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/bootstrap-typeahead.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-typeahead.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/jquery-1.7.2.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/jquery-1.7.2.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/jquery-1.7.2.min.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/jquery-1.7.2.min.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/js/jquery.md5.js');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/js/jquery.md5.js?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/accordion.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/accordion.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/alerts.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/alerts.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/breadcrumbs.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/breadcrumbs.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/button-groups.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/button-groups.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/buttons.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/buttons.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/carousel.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/carousel.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/close.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/close.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/code.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/code.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/colorpicker.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/colorpicker.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/component-animations.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/component-animations.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/datepicker.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/datepicker.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/dropdowns.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/dropdowns.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/forms.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/forms.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/grid.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/grid.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/hero-unit.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/hero-unit.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/labels-badges.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/labels-badges.less?r\x3d481\x26spec\x3dsvn481');
 
 
 changed_paths.push('/branches/dev-2.0/include/less/layouts.less');
 changed_urls.push('/p/phpwcms/source/browse/branches/dev-2.0/include/less/layouts.less?r\x3d481\x26spec\x3dsvn481');
 
 
 function getCurrentPageIndex() {
 for (var i = 0; i < changed_paths.length; i++) {
 if (selected_path == changed_paths[i]) {
 return i;
 }
 }
 }
 function getNextPage() {
 var i = getCurrentPageIndex();
 if (i < changed_paths.length - 1) {
 return changed_urls[i + 1];
 }
 return null;
 }
 function getPreviousPage() {
 var i = getCurrentPageIndex();
 if (i > 0) {
 return changed_urls[i - 1];
 }
 return null;
 }
 function gotoNextPage() {
 var page = getNextPage();
 if (!page) {
 page = detail_url;
 }
 window.location = page;
 }
 function gotoPreviousPage() {
 var page = getPreviousPage();
 if (!page) {
 page = detail_url;
 }
 window.location = page;
 }
 function gotoDetailPage() {
 window.location = detail_url;
 }
 function gotoPublishPage() {
 window.location = publish_url;
 }
</script>

 
 <style type="text/css">
 #review_nav {
 border-top: 3px solid white;
 padding-top: 6px;
 margin-top: 1em;
 }
 #review_nav td {
 vertical-align: middle;
 }
 #review_nav select {
 margin: .5em 0;
 }
 </style>
 <div id="review_nav">
 <table><tr><td>Go to:&nbsp;</td><td>
 <select name="files_in_rev" onchange="window.location=this.value">
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/COMPONENTS.md?r=481&amp;spec=svn481"
 
 >/branches/dev-2.0/COMPONENTS.md</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/GPL?r=481&amp;spec=svn481"
 
 >/branches/dev-2.0/GPL</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/LICENSE?r=481&amp;spec=svn481"
 
 >/branches/dev-2.0/LICENSE</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/README.md?r=481&amp;spec=svn481"
 
 >/branches/dev-2.0/README.md</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include?r=481&amp;spec=svn481"
 
 >/branches/dev-2.0/include</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/css/colorpicker.css?r=481&amp;spec=svn481"
 
 >...-2.0/include/css/colorpicker.css</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/css/datepicker.css?r=481&amp;spec=svn481"
 
 >...v-2.0/include/css/datepicker.css</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/css/phpwcms-v2-responsive.css?r=481&amp;spec=svn481"
 
 >...de/css/phpwcms-v2-responsive.css</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/css/phpwcms-v2.css?r=481&amp;spec=svn481"
 
 >...v-2.0/include/css/phpwcms-v2.css</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/img/alpha.png?r=481&amp;spec=svn481"
 
 >...es/dev-2.0/include/img/alpha.png</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/img/glyphicons-halflings-white.png?r=481&amp;spec=svn481"
 
 >...g/glyphicons-halflings-white.png</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/img/glyphicons-halflings.png?r=481&amp;spec=svn481"
 
 >...ude/img/glyphicons-halflings.png</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/img/hue.png?r=481&amp;spec=svn481"
 
 >...ches/dev-2.0/include/img/hue.png</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/img/phpwcms-signet-login.png?r=481&amp;spec=svn481"
 
 >...ude/img/phpwcms-signet-login.png</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/img/saturation.png?r=481&amp;spec=svn481"
 
 >...v-2.0/include/img/saturation.png</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-alert.js?r=481&amp;spec=svn481"
 
 >....0/include/js/bootstrap-alert.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-button.js?r=481&amp;spec=svn481"
 
 >...0/include/js/bootstrap-button.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-carousel.js?r=481&amp;spec=svn481"
 
 >...include/js/bootstrap-carousel.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-collapse.js?r=481&amp;spec=svn481"
 
 >...include/js/bootstrap-collapse.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-colorpicker.js?r=481&amp;spec=svn481"
 
 >...lude/js/bootstrap-colorpicker.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-datepicker.js?r=481&amp;spec=svn481"
 
 >...clude/js/bootstrap-datepicker.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-dropdown.js?r=481&amp;spec=svn481"
 
 >...include/js/bootstrap-dropdown.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-modal.js?r=481&amp;spec=svn481"
 
 >....0/include/js/bootstrap-modal.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-popover.js?r=481&amp;spec=svn481"
 
 >.../include/js/bootstrap-popover.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-scrollspy.js?r=481&amp;spec=svn481"
 
 >...nclude/js/bootstrap-scrollspy.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-tab.js?r=481&amp;spec=svn481"
 selected="selected"
 >...-2.0/include/js/bootstrap-tab.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-tooltip.js?r=481&amp;spec=svn481"
 
 >.../include/js/bootstrap-tooltip.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-transition.js?r=481&amp;spec=svn481"
 
 >...clude/js/bootstrap-transition.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/bootstrap-typeahead.js?r=481&amp;spec=svn481"
 
 >...nclude/js/bootstrap-typeahead.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/jquery-1.7.2.js?r=481&amp;spec=svn481"
 
 >...v-2.0/include/js/jquery-1.7.2.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/jquery-1.7.2.min.js?r=481&amp;spec=svn481"
 
 >...0/include/js/jquery-1.7.2.min.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/js/jquery.md5.js?r=481&amp;spec=svn481"
 
 >...dev-2.0/include/js/jquery.md5.js</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less?r=481&amp;spec=svn481"
 
 >/branches/dev-2.0/include/less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/accordion.less?r=481&amp;spec=svn481"
 
 >...-2.0/include/less/accordion.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/alerts.less?r=481&amp;spec=svn481"
 
 >...dev-2.0/include/less/alerts.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/breadcrumbs.less?r=481&amp;spec=svn481"
 
 >....0/include/less/breadcrumbs.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/button-groups.less?r=481&amp;spec=svn481"
 
 >.../include/less/button-groups.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/buttons.less?r=481&amp;spec=svn481"
 
 >...ev-2.0/include/less/buttons.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/carousel.less?r=481&amp;spec=svn481"
 
 >...v-2.0/include/less/carousel.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/close.less?r=481&amp;spec=svn481"
 
 >.../dev-2.0/include/less/close.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/code.less?r=481&amp;spec=svn481"
 
 >...s/dev-2.0/include/less/code.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/colorpicker.less?r=481&amp;spec=svn481"
 
 >....0/include/less/colorpicker.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/component-animations.less?r=481&amp;spec=svn481"
 
 >...e/less/component-animations.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/datepicker.less?r=481&amp;spec=svn481"
 
 >...2.0/include/less/datepicker.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/dropdowns.less?r=481&amp;spec=svn481"
 
 >...-2.0/include/less/dropdowns.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/forms.less?r=481&amp;spec=svn481"
 
 >.../dev-2.0/include/less/forms.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/grid.less?r=481&amp;spec=svn481"
 
 >...s/dev-2.0/include/less/grid.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/hero-unit.less?r=481&amp;spec=svn481"
 
 >...-2.0/include/less/hero-unit.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/labels-badges.less?r=481&amp;spec=svn481"
 
 >.../include/less/labels-badges.less</option>
 
 <option value="/p/phpwcms/source/browse/branches/dev-2.0/include/less/layouts.less?r=481&amp;spec=svn481"
 
 >...ev-2.0/include/less/layouts.less</option>
 
 </select>
 </td></tr></table>
 
 
 



 
 </div>
 
 
 </div>
 <div class="round1"></div>
 <div class="round2"></div>
 <div class="round4"></div>
 </div>
 <div class="pmeta_bubble_bg" style="border:1px solid white">
 <div class="round4"></div>
 <div class="round2"></div>
 <div class="round1"></div>
 <div class="box-inner">
 <div id="older_bubble">
 <p>Older revisions</p>
 
 <a href="/p/phpwcms/source/list?path=/branches/dev-2.0/include/js/bootstrap-tab.js&start=481">All revisions of this file</a>
 </div>
 </div>
 <div class="round1"></div>
 <div class="round2"></div>
 <div class="round4"></div>
 </div>
 
 <div class="pmeta_bubble_bg" style="border:1px solid white">
 <div class="round4"></div>
 <div class="round2"></div>
 <div class="round1"></div>
 <div class="box-inner">
 <div id="fileinfo_bubble">
 <p>File info</p>
 
 <div>Size: 3384 bytes,
 135 lines</div>
 
 <div><a href="//phpwcms.googlecode.com/svn-history/r481/branches/dev-2.0/include/js/bootstrap-tab.js">View raw file</a></div>
 </div>
 
 <div id="props">
 <p>File properties</p>
 <dl>
 
 <dt>svn:eol-style</dt>
 <dd>native</dd>
 
 <dt>svn:keywords</dt>
 <dd>Id Author Revision Date</dd>
 
 </dl>
 </div>
 
 </div>
 <div class="round1"></div>
 <div class="round2"></div>
 <div class="round4"></div>
 </div>
 </div>
 </div>


</div>

</div>
</div>

<script src="http://www.gstatic.com/codesite/ph/4212538301465177006/js/prettify/prettify.js"></script>
<script type="text/javascript">prettyPrint();</script>


<script src="http://www.gstatic.com/codesite/ph/4212538301465177006/js/source_file_scripts.js"></script>

 <script type="text/javascript" src="http://www.gstatic.com/codesite/ph/4212538301465177006/js/kibbles.js"></script>
 <script type="text/javascript">
 var lastStop = null;
 var initialized = false;
 
 function updateCursor(next, prev) {
 if (prev && prev.element) {
 prev.element.className = 'cursor_stop cursor_hidden';
 }
 if (next && next.element) {
 next.element.className = 'cursor_stop cursor';
 lastStop = next.index;
 }
 }
 
 function pubRevealed(data) {
 updateCursorForCell(data.cellId, 'cursor_stop cursor_hidden');
 if (initialized) {
 reloadCursors();
 }
 }
 
 function draftRevealed(data) {
 updateCursorForCell(data.cellId, 'cursor_stop cursor_hidden');
 if (initialized) {
 reloadCursors();
 }
 }
 
 function draftDestroyed(data) {
 updateCursorForCell(data.cellId, 'nocursor');
 if (initialized) {
 reloadCursors();
 }
 }
 function reloadCursors() {
 kibbles.skipper.reset();
 loadCursors();
 if (lastStop != null) {
 kibbles.skipper.setCurrentStop(lastStop);
 }
 }
 // possibly the simplest way to insert any newly added comments
 // is to update the class of the corresponding cursor row,
 // then refresh the entire list of rows.
 function updateCursorForCell(cellId, className) {
 var cell = document.getElementById(cellId);
 // we have to go two rows back to find the cursor location
 var row = getPreviousElement(cell.parentNode);
 row.className = className;
 }
 // returns the previous element, ignores text nodes.
 function getPreviousElement(e) {
 var element = e.previousSibling;
 if (element.nodeType == 3) {
 element = element.previousSibling;
 }
 if (element && element.tagName) {
 return element;
 }
 }
 function loadCursors() {
 // register our elements with skipper
 var elements = CR_getElements('*', 'cursor_stop');
 var len = elements.length;
 for (var i = 0; i < len; i++) {
 var element = elements[i]; 
 element.className = 'cursor_stop cursor_hidden';
 kibbles.skipper.append(element);
 }
 }
 function toggleComments() {
 CR_toggleCommentDisplay();
 reloadCursors();
 }
 function keysOnLoadHandler() {
 // setup skipper
 kibbles.skipper.addStopListener(
 kibbles.skipper.LISTENER_TYPE.PRE, updateCursor);
 // Set the 'offset' option to return the middle of the client area
 // an option can be a static value, or a callback
 kibbles.skipper.setOption('padding_top', 50);
 // Set the 'offset' option to return the middle of the client area
 // an option can be a static value, or a callback
 kibbles.skipper.setOption('padding_bottom', 100);
 // Register our keys
 kibbles.skipper.addFwdKey("n");
 kibbles.skipper.addRevKey("p");
 kibbles.keys.addKeyPressListener(
 'u', function() { window.location = detail_url; });
 kibbles.keys.addKeyPressListener(
 'r', function() { window.location = detail_url + '#publish'; });
 
 kibbles.keys.addKeyPressListener('j', gotoNextPage);
 kibbles.keys.addKeyPressListener('k', gotoPreviousPage);
 
 
 }
 </script>
<script src="http://www.gstatic.com/codesite/ph/4212538301465177006/js/code_review_scripts.js"></script>
<script type="text/javascript">
 function showPublishInstructions() {
 var element = document.getElementById('review_instr');
 if (element) {
 element.className = 'opened';
 }
 }
 var codereviews;
 function revsOnLoadHandler() {
 // register our source container with the commenting code
 var paths = {'svn481': '/branches/dev-2.0/include/js/bootstrap-tab.js'}
 codereviews = CR_controller.setup(
 {"profileUrl":"/u/112436863544550044548/","token":"Lw_q3elB2KxwGPLvKAovdZ9PG64:1351385300363","assetHostPath":"http://www.gstatic.com/codesite/ph","domainName":null,"assetVersionPath":"http://www.gstatic.com/codesite/ph/4212538301465177006","projectHomeUrl":"/p/phpwcms","relativeBaseUrl":"","projectName":"phpwcms","loggedInUserEmail":"tripathi.smriti@gmail.com"}, '', 'svn481', paths,
 CR_BrowseIntegrationFactory);
 
 codereviews.registerActivityListener(CR_ActivityType.REVEAL_DRAFT_PLATE, showPublishInstructions);
 
 codereviews.registerActivityListener(CR_ActivityType.REVEAL_PUB_PLATE, pubRevealed);
 codereviews.registerActivityListener(CR_ActivityType.REVEAL_DRAFT_PLATE, draftRevealed);
 codereviews.registerActivityListener(CR_ActivityType.DISCARD_DRAFT_COMMENT, draftDestroyed);
 
 
 
 
 
 
 
 var initialized = true;
 reloadCursors();
 }
 window.onload = function() {keysOnLoadHandler(); revsOnLoadHandler();};

</script>
<script type="text/javascript" src="http://www.gstatic.com/codesite/ph/4212538301465177006/js/dit_scripts.js"></script>

 
 
 
 <script type="text/javascript" src="http://www.gstatic.com/codesite/ph/4212538301465177006/js/ph_core.js"></script>
 
 
 
 
</div> 

<div id="footer" dir="ltr">
 <div class="text">
 <a href="/projecthosting/terms.html">Terms</a> -
 <a href="http://www.google.com/privacy.html">Privacy</a> -
 <a href="/p/support/">Project Hosting Help</a>
 </div>
</div>
 <div class="hostedBy" style="margin-top: -20px;">
 <span style="vertical-align: top;">Powered by <a href="http://code.google.com/projecthosting/">Google Project Hosting</a></span>
 </div>

 
 


 
 </body>
</html>

