import { App } from "./app";
import jQuery from 'jquery'
window.$ = jQuery;
window.jQuery = jQuery;
import "foundation-sites/dist/css/foundation.css";
import "toastr/build/toastr.css";
import "./site.css";
import * as Toastr from 'toastr';

var startX, startWidth, startHeight;
let sidebar, resizer;

function initDrag(e) {
    startX = e.clientX;
    startWidth = parseInt(document.defaultView.getComputedStyle(sidebar).width, 10);
    document.documentElement.addEventListener('mousemove', doDrag, false);
    document.documentElement.addEventListener('mouseup', stopDrag, false);
}

function doDrag(e) {
    sidebar.style.width = (startWidth - e.clientX + startX) + 'px';
}

function stopDrag(e) {
    document.documentElement.removeEventListener('mousemove', doDrag, false); document.documentElement.removeEventListener('mouseup', stopDrag, false);
}

function initialize() {
    resizer = document.getElementById('resizer');
    sidebar = document.getElementById('sidebar');
    resizer.addEventListener('mousedown', initDrag, false);
}

initialize();
let app = new App();
app.start();