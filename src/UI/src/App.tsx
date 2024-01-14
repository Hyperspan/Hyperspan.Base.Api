import { BrowserRouter, Route, Routes } from "react-router-dom";
import Layout from "./components/Layout/Layout";
import FileDrive from "./modules/FileDrive";

export default function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route element={<Layout />}>
            <Route path="/" element={<FileDrive />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </>
  );
}
