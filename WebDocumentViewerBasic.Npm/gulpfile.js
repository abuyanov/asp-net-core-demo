/// <binding BeforeBuild='build' Clean='clean' />
"use strict";

const gulp = require("gulp");
const rimraf = require("rimraf");

const libPath = 'wwwroot/lib/'; //Path where all dependencies should be copied
const nodeModules = "node_modules/"; //Folder where node moduels stored by default

//List of dependecies for our application
//src - list of files of wildcard paths to files to copy
//dest - folder name of path relative to libpath variable where files should be copied
const dependencies = [
    {
        src: ["jquery/dist/jquery.js", "jquery/dist/jquery.min.js"],
        dest: "jquery"
    },
    {
        src: ["jquery.easing/jquery.easing.js","jquery.easing/jquery.easing.min.js"],
        dest: "jquery-easing"
    },
    {
        src: ["jquery-ui-dist/jquery-ui.js", "jquery-ui-dist/jquery-ui.min.js"],
        dest: "jquery-ui"
    },
    {
        src: ["jquery-ui-dist/jquery-ui.css", "jquery-ui-dist/jquery-ui.min.css"],
        dest: "jquery-ui/css"
    },
    {
        src: ["raphael/raphael.js","raphael/raphael.min.js"],
        dest: "raphael"
    },
    {
        src: ["clipboard/dist/*"],
        dest: "clipboard"
    },
    {
        src: ["web-document-viewer/*.js", "web-document-viewer/*.css"],
        dest: "web-document-viewer"
    },
    {
        src: ["web-document-viewer/images/*"],
        dest: "web-document-viewer/images"
    }
];

function getSourcePaths(rootpath, paths) {
    return paths.map(path => rootpath + path);
}

gulp.task("build",
    (done) => {
        dependencies.forEach(dep => {
            gulp.src(getSourcePaths(nodeModules, dep.src)).pipe(gulp.dest(`${libPath}/${dep.dest}`));
        });
        done();
    });

gulp.task("clean",
    (done) => {
        return rimraf(libPath, done);
    });

gulp.task("rebuild", gulp.series("clean","build"));