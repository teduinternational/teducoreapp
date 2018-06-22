var gulp = require("gulp"),
  rimraf = require("rimraf"),
  concat = require("gulp-concat"),
  cssmin = require("gulp-cssmin"),
  uglify = require("gulp-uglify");

var paths = {
    webroot: "./wwwroot/"
};

paths.adminJs = paths.webroot + "app/**/*.js";
paths.clientJs = paths.webroot + "client-app/**/*.js";

paths.minJs = paths.webroot + "**/*.min.js";

paths.adminCss = paths.webroot + "admin-side/**/*.css";
paths.clientCss = paths.webroot + "client-side/**/*.css";

paths.minCss = paths.webroot + "**/*.min.css";

paths.concatJsAdminDest = paths.webroot + "bundled/site-admin.min.js";
paths.concatJsClientDest = paths.webroot + "bundled/site-client.min.js";

paths.concatAdminCssDest = paths.webroot + "bundled/site-admin.min.css";
paths.concatClientCssDest = paths.webroot + "bundled/site-client.min.css";

gulp.task("clean:adminJs", function (cb) {
    rimraf(paths.concatJsAdminDest, cb);
});

gulp.task("clean:clientJs", function (cb) {
    rimraf(paths.concatJsClientDest, cb);
});

gulp.task("clean:adminCss", function (cb) {
    rimraf(paths.concatAdminCssDest, cb);
});


gulp.task("clean:clientCss", function (cb) {
    rimraf(paths.concatClientCssDest, cb);
});

gulp.task("clean", ["clean:adminJs", "clean:clientJs", "clean:adminCss", "clean:clientCss"]);

gulp.task("min:adminJs", function () {
    return gulp.src([paths.adminJs, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsAdminDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:clientJs", function () {
    return gulp.src([paths.clientJs, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsClientDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});


gulp.task("min:adminCss", function () {
    return gulp.src([paths.adminCss, "!" + paths.minCss])
        .pipe(concat(paths.concatAdminCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});
gulp.task("min:clientCss", function () {
    return gulp.src([paths.clientCss, "!" + paths.minCss])
        .pipe(concat(paths.concatClientCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:adminJs", "min:clientJs", "min:adminCss", "min:clientCss"]);