// store/authSlice.ts
import { createSlice, type PayloadAction } from "@reduxjs/toolkit";

interface AuthState {
  //token: string | null;
  lastName: string | null;
  firstName: string | null;
  middleName: string | null;
  role: string | null;
  organizationId: number | null;
  isBlocked: boolean | null;
}

const initialState: AuthState = {
  //token: null,
  lastName: null,
  firstName: null,
  middleName: null,
  role: null,
  organizationId: null,
  isBlocked: null,
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
      state.role = info.role || null;
      state.organizationId = info.organizationId || null;
      state.isBlocked = info.isBlocked || null;

    },
    logout(state) {
      //state.token = null;
      state.organizationId = null;
      state.role = null;
      state.isBlocked = null;
    },
  },
});

export const { setInfo, logout } = authSlice.actions;
export default authSlice.reducer;
