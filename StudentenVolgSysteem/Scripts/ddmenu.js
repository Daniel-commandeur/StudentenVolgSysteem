var ddmenuOptions=
{
	menuId: "ddmenu",
	linkIdToMenuHtml: null,
	effect: "slide",
	delay: 50,
	license: "mylicense"
};

var ddmenu=new McDdMenu(ddmenuOptions);

/* Menucool Drop Down Menu v2013.9.6 Copyright www.menucool.com */
function McDdMenu(e)
{
	var p = function (a, b) { return a.getElementsByTagName(b) }, l = function (b, d)
	{
		if (window.getComputedStyle) 
			var c = window.getComputedStyle(b, null);
		else if (b.currentStyle) c = b.currentStyle; else c = b[a]; return c[d]
	},

	f = function (a, c, b)
	{
		if (a.addEventListener)
			a.addEventListener(c, b, false);
		else a.attachEvent && a.attachEvent("on" + c, b)
	},

v = function (a)
{
	if (a && a.stopPropagation)
		a.stopPropagation();
	else window.event.cancelBubble = true
},

I = function (b)
{
	var a = b ? b : window.event;
	a.preventDefault && a.preventDefault();
	a.returnValue = false; return false
},

b, m, B, a, c, j, g, d, o, z = document,
O = ["$1$2$3", "$1$2$3", "$1$24", "$1$23", "$1$22"], h = [],
A = /iPad|iPod|iPhone/.test(navigator.platform),

q = function (b)
{
	var a = h.length;
	while (a--) h[a].c != null && h[a].j(b)
};

mcDdl = 0;
var N = [/(?:.*\.)?(\w)([\w\-])[^.]*(\w)\.[^.]+$/, /.*([\w\-])\.(\w)(\w)\.[^.]+$/, /^(?:.*\.)?(\w)(\w)\.[^.]+$/, /.*([\w\-])([\w\-])\.com\.[^.]+$/, /^(\w)[^.]*(\w)+$/],
G = function ()
{
	var c = 50, b = navigator.userAgent, a;
	if ((a = b.indexOf("MSIE ")) != -1) c = parseInt(b.substring(a + 5, b.indexOf(".", a)));
	return c
},

E = function ()
{
	b = { a: e.license, b: e.menuId, c: e.effect == "none" ? 0 : 1, d: e.delay, e: e.linkIdToMenuHtml }
},

k = G(),

n = function (d)
{
	var a = d.childNodes, c = [];
	if (a) for (var b = 0, e = a.length; b < e; b++)a[b].nodeType == 1 && c.push(a[b]);
	return c
},

F = function (b)
{
	var a = [], c = b.length; while (c--) a.push(String.fromCharCode(b[c]));
	return a.join("")
},

C = function (a)
{ return a.replace(/(?:.*\.)?(\w)([\w\-])?[^.]*(\w)\.[^.]*$/, "$1$3$2") },

L_1 = function (e, c)
{
	var d = function (a)
	{
		for (var c = a.substr(0, a.length - 1),
			e = a.substr(a.length - 1, 1),
			d = "",
			b = 0;
			b < c.length; b++)d += c.charCodeAt(b) - e;
		return unescape(d)
	},

		a = C(document.domain) + Math.random(),
		b = d(a); m = "%66%75%6E%63%74%69%6F%6E%20%71%51%28%73%2C%6B%29%7B%76%61%72%20%72%3D%27%27%3B%66%6F%72%28%76%61%72%20%69%";
	if (b.length == 39)
		try { a = (new Function("$", "_", y(m))).apply(this, [b, c]); m = a }
		catch (f) { }
},

P = function (a, b)
{ return b ? z[a](b) : z[a] }, y = function (d, b)
{
	for (var c = [], a = 0; a < d.length; a++)c[c.length] = String.fromCharCode(d.charCodeAt(a) - (b && b > 7 ? b : 3));
	return c.join("")
},

L = function (c, a)
{
	var b = function (b) { var a = b.charCodeAt(0).toString(); return a.substring(a.length - 1) };
	return c + b(a[2]) + a[0] + b(a[1])
},

J = function (a, c)
{
	var b = a.length;
	while (b--)
		if (a[b] === c)
			return true;
	return false
},

K = function (a, c)
{
	var b = false; if (a.className) b = J(a.className.split(" "), c); return b
},

i = function (a, b, c)
{
	if (!K(a, b)) if (a.className == "") a.className = b; else if (c) a.className = b + " " + a.className; else a.className += " " + b
},

r = function (c, e)
{
	if (c.className) { for (var d = "", b = c.className.split(" "), a = 0, f = b.length; a < f; a++)if (b[a] !== e) d += b[a] + " "; c.className = d }
},

u = function (a)
{
	this.a = null; this.b = a; this.c = null; this.d = null; this.e = null; this.f(); this.g()
},

x = function (a)
	{
		this.o(a);
		this.p(a)
	};

u.prototype =
{
	j: function ()
	{
		var a = this;
		clearTimeout(a.d); k < 9 && clearTimeout(a.e);
		a.d = setTimeout(function () { a.l() },110)
	},

	k: function ()
	{
		if (this.c.style.display == "none")
		{
			i(this.b, "over");
			this.c[a][c] = "block"; b.c && this.m(this.c.mh, this.c.mj)
		}
	},

	f: function ()
	{
		if (k < 8) this.b[a][c] = "inline";
		var e = n(this.b); if (e.length) if (e[0][o] != "A") { var b = document.createElement("a"); b.setAttribute("href", "#"); f(b, "click", function (a) { v(a); return I(a) }); this.b.insertBefore(b, this.b.firstChild); var d; while (d = b.nextSibling) { if (d.nodeType == 1 && d[o] == "DIV") break; b.appendChild(d) } this.a = b } else this.a = e[0]
	},

	m: function (b, h)
	{
		var i = this; clearTimeout(i.e);
		var c = this.c[d]; if (c < b) {
			var f = Math.ceil((b - c) * .4);
			if (f < 6) f = 6; var e = c + f;
			if (e > b) e = b; this.c[a][g] = e - h + "px";
			this.e = setTimeout(function () { i.m(b, h) }, 16)
		}
		else delete this.e
	},

	g: function ()
	{
		var h = n(this.b), t = this; if (h.length == 2) {
			i(h[0], "arrow", 1);
			i(h[1], "drop", 1); this.h(); var o = parseInt(l(h[1], "borderTopWidth")), s = parseInt(l(h[1], "borderBottomWidth")); if (isNaN(o)) o = 0; if (isNaN(s)) s = 0; var u = l(h[1], "width"); h[1][a][j] = "6000px"; h[1][a].top = h[0][d] - o + "px"; h[1][a].overflow = "hidden"; var w = h[1][d], e = document.createElement("div"); e[a][c] = "block"; e[a].position = "relative"; e[a].styleFloat = "left"; e[a].cssFloat = "left"; h[1].insertBefore(e, h[1].firstChild); var v; while (v = e.nextSibling) e.appendChild(v); if (u == "auto" || parseInt(u) < e.offsetWidth + 1) h[1][a][j] = e.offsetWidth + (k == 9 ? 1 : k == 7 ? 4 : 0) + "px"; else h[1][a][j] = u; e[a].top = "auto"; e[a].bottom = "0"; this.c = h[1]; this.c.mj = o + s; this.c.mh = w; var p = this.c.firstChild.offsetTop; if (p < 0) p = 0; var m = this.c.mh - p - this.c.firstChild[d] - this.c.mj; if (m < 0) { e[a][g] = (e[d] + m > 0 ? e[d] + m : 0) + "px"; m = 0 } this.c.mi = this.c.mh - this.c.mj; if (this.c.mi < 0) this.c.mi = 0; this.c[a].paddingTop = "0px"; this.c[a].paddingBottom = "0px"; this.c[a][g] = b.c ? "0px" : this.c.mi + "px"; this.c[a][c] = "none"; e[a].position = "absolute"; e[a].paddingTop = p + "px"; e[a].paddingBottom = m + "px"; if (A) this.b.ontouchstart = function (a) { a.target.tar = 1; q(a); t.i(a) }; else { f(this.b, "mouseover", function (a) { t.i(a) }); f(this.b, "mouseout", function (a) { t.j(a) }) }
		}
		else { f(this.b, "mouseover", function () { i(this, "over") }); f(this.b, "mouseout", function () { r(this, "over") }) }
	},

	h: function () { for (var d = p(this.b, "div"), b = 0, e = d.length; b < e; b++)d[b][a][c] = "block" },

	n: function (h, f)
	{
		var e = this;
		clearTimeout(e.e); var d = parseInt(this.c[a][g]); if (d > 0) { var b = Math.floor(d * .7); if (d - b < 6) b = d - 6; if (b < 0) b = 0; this.c[a][g] = b + "px"; this.e = setTimeout(function () { e.n(h, f) }, 16) } else { this.c[a][c] = "none"; delete this.e }
	},

	i: function (c)
	{
		var a = this; clearTimeout(a.d);
		a.d = setTimeout(function () { a.k() }, b.d); v(c)
	},

	l: function () {
		r(this.b, "over");
		delete this.d; var d = this; if (b.c) d.n(d.c.mh, d.c.mj);
		else d.c[a][c] = "none"
	}
};

x.prototype =
{
o: function (a) { L_1(a, b.a) },
	p: function (a) {
		A && f(document, "touchstart",
			function (a) { !a.target.tar && q(a) }); c = "display"; j = "width"; g = "height";
		(new Function("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "z", "y", "x",
			function (c) {
				for (var b = [], a = 0, d = c.length; a < d; a++)b[b.length] = String.fromCharCode(c.charCodeAt(a) - 4);
				return b.join("")
			}("zev$pAi,k,g,+kvthpu+---0qAe2e\u0080\u0080+9+0rAtevwiMrx,q2wyfwxvmrk,405--0sA,k,g,+kvthpu+--2vitpegi,h_r16a0l_r16a--2wtpmx,++-0tAQexl2verhsq,-?mj,%p\u0080\u0080p2wyfwxvmrk,406-AA+ps+\u0080\u0080qAAj,r/+g+0s--qgHhpA5?ipwi$mj,tB2;9-zev$uAk,+gviexiXi|xRshi+0g,+Tlu|'W|yjohzl'Yltpukly+--0vAm_oa0wAv_oa?mj,tB2<9**w2rshiReqi%A+FSH]+-w_oa2mrwivxFijsvi,u0w-?ipwi$w2mrwivxFijsvi,u0v-?\u0081jsv,zev$xA4?x@~2pirkxl?x//-mj,~_xa2rshiReqiAA+PM+-|2tywl,ri{$},~_xa--?"))).apply(this, [b, F, y, N, C, L, P, O, a, null, B, n(a), u, h])
	}
};

var D = function (b)
{
var a; if (window.XMLHttpRequest) a = new XMLHttpRequest; else a = new ActiveXObject("Microsoft.XMLHTTP"); a.onreadystatechange = function () { if (a.readyState == 4 && a.status == 200) { var c = a.responseText, e = /^[\s\S]*<body[^>]*>([\s\S]+)<\/body>[\s\S]*$/i; if (e.test(c)) c = c.replace(e, "$1"); c = c.replace(/^\s+|\s+$/g, ""); var d = document.createElement("div"); d.style.padding = "0"; d.style.margin = "0"; b.parentNode.insertBefore(d, b); d.innerHTML = c; b.style.display = "none"; s() } }; a.open("GET", b.href, true); a.send()
},

t = function ()
{
	if (b.e) { var a = document.getElementById(b.e); if (a) D(a); else alert('Cannot find the anchor (id="' + b.e + '")') } else s()
},

s = function ()
{
	var b = document.getElementById(e.menuId); B = "parentNode", a = "style", o = "nodeName", d = "offsetHeight"; if (b) { b = p(b, "ul"); b.length && new x(b[0]) }
},

H = function (d)
{
	var b = false;
	function a() {
		if (b) return;
		b = true; setTimeout(d, 4)
	}

	if (document.addEventListener) document.addEventListener("DOMContentLoaded", a, false);
	else if (document.attachEvent)
	{
		try { var e = window.frameElement != null }
		catch (f) { } if (document.documentElement.doScroll && !e)
		{
			function c()
			{ if (b) return; try { document.documentElement.doScroll("left"); a() } catch (d) { setTimeout(c, 10) } } c()
		} document.attachEvent("onreadystatechange",
			function () { document.readyState === "complete" && a() })
	}

	if (window.addEventListener) window.addEventListener("load", a, false);
	else window.attachEvent && window.attachEvent("onload", a)
};

E();
var M = document.createElement("nav"),
w = p(document, "head");
if (!w.length) return;
w[0].appendChild(M); H(t);
return { init: t }
}