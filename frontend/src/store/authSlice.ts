// store/authSlice.ts
import { createSlice, type PayloadAction } from "@reduxjs/toolkit";

interface AuthState {
  //token: string | null;
  lastName: string | null;
  firstName: string | null;
  middleName: string | null;
  organizationId: number | null;
}

const initialState: AuthState = {
  //token: null,
  lastName: null,
  firstName: null,
  middleName: null,
  organizationId: null,
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setInfo(state, action: PayloadAction<string>) {
      const info = JSON.parse(action.payload);
      //state.token = token;
      state.lastName = info.lastName || null;
      state.firstName = info.firstName || null;
      state.middleName = info.middleName || null;
      state.organizationId = info.organizationId || null;

    },
    logout(state) {
      //state.token = null;
      state.organizationId = null;
    },
  },
});

export const { setInfo, logout } = authSlice.actions;
export default authSlice.reducer;
