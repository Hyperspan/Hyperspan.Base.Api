import { useSelector } from "react-redux";
import { RootState } from "../../helpers/store";

export default function Loader() {
  const loaderArray = useSelector((rootState: RootState) => rootState.DriveReducer.loader);

  return (
    <>
      {loaderArray.length > 0 && (
        <div className="loader-container">
          <div className="loader-outer" />
        </div>
      )}
    </>
  );
}
