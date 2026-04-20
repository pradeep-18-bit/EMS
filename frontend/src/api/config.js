const trimTrailingSlashes = (value) => value.replace(/\/+$/, "");

const configuredApiOrigin = import.meta.env.VITE_API_ORIGIN?.trim();

const API_ORIGIN = trimTrailingSlashes(
  configuredApiOrigin ||
    (import.meta.env.DEV ? "http://localhost:5007" : window.location.origin)
);

export const SERVER_URL = API_ORIGIN;
export const BASE_URL = `${API_ORIGIN}/api`;
