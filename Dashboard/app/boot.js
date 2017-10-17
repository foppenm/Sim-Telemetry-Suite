import { App } from "./app";
import { paper } from "paper";

import "foundation-sites/dist/css/foundation.css";

import jQuery from 'jquery'
window.$ = jQuery;
window.jQuery = jQuery;

let canvas = document.getElementById('myCanvas');
paper.setup(canvas);            // Create an empty project and a view for the canvas

let app = new App();
app.start();