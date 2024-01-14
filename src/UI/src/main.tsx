import "bootstrap/dist/css/bootstrap.min.css";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./assets/css/index.css";
import { Provider } from "react-redux";
import { store } from "./helpers/store.ts";
import UploadFile from "./modules/UploadFile.tsx";
import Loader from "./components/Loader/Loader.tsx";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <Provider store={store}>
    <App />
    <UploadFile />
    <Loader />
  </Provider>,
);
