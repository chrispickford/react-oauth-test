/*global __dirname */
const path = require("path");
const webpack = require("webpack");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CleanWebpackPlugin = require("clean-webpack-plugin");

module.exports = (env, opts) => {
  return {
    module: {
      rules: [
        {
          test: /\.tsx?$/,
          exclude: /node_modules/,
          loader: "awesome-typescript-loader"
        },
        {
          test: /\.(scss|css)$/,

          use: [
            {
              loader: MiniCssExtractPlugin.loader
            },
            {
              loader: "css-loader",

              options: {
                sourceMap: true
              }
            },
            {
              loader: "sass-loader",

              options: {
                sourceMap: true
              }
            }
          ]
        }
      ]
    },

    plugins: [
      new webpack.ProgressPlugin(),
      new webpack.NoEmitOnErrorsPlugin(),
      new HtmlWebpackPlugin({
        template: "./src/index.html"
      }),
      new CleanWebpackPlugin(["wwwroot"], { verbose: true, beforeEmit: true }),
      new UglifyJsPlugin({ sourceMap: true }),
      new MiniCssExtractPlugin({ filename: "styles.[chunkhash].css" })
    ],

    entry: {
      app: ["./src/main"]
    },

    output: {
      path: path.resolve(__dirname, "wwwroot"),
      filename: "[name].[chunkhash].js",
      chunkFilename: "[name].[chunkhash].js",
      publicPath: "/"
    },

    devtool: "source-map",

    optimization: {
      splitChunks: {
        cacheGroups: {
          commons: { test: /[\\/]node_modules[\\/]/, name: "vendors", chunks: "all" }
        }
      }
    },

    resolve: {
      extensions: [".ts", ".tsx", ".js", ".json"]
    },

    mode: opts && opts.mode ? opts.mode : "development"
  };
};
