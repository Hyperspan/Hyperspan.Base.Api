import { configureStore } from "@reduxjs/toolkit";
import DriveReducer from "../redux/DriveReducer";

export const store = configureStore({
  reducer: {
    DriveReducer: DriveReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;
