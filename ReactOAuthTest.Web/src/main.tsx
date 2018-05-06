import * as React from "react";
import * as ReactDOM from "react-dom";

class App extends React.Component {
  render() {
    return (
      <div>
        <p>Hello World!</p>
      </div>
    );
  }
}

ReactDOM.render(<App/>, document.getElementById("app"));
