import { afterEach, describe, expect, it, vi } from 'vitest'
import { getApiBaseUrl } from './config.ts'

describe('getApiBaseUrl', () => {
  afterEach(() => {
    vi.unstubAllEnvs()
  })

  it('returns the configured base URL without one trailing slash', () => {
    vi.stubEnv('VITE_API_BASE_URL', 'http://localhost:8080/')
    expect(getApiBaseUrl()).toBe('http://localhost:8080')
  })

  it('returns an empty string when the variable is unset', () => {
    vi.stubEnv('VITE_API_BASE_URL', '')
    expect(getApiBaseUrl()).toBe('')
  })
})
