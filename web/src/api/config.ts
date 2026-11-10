export function getApiBaseUrl(): string {
  const value = import.meta.env.VITE_API_BASE_URL
  if (!value) {
    return ''
  }

  return value.replace(/\/$/, '')
}
